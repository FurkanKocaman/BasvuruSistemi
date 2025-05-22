using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Roles;
using BasvuruSistemi.Server.Domain.Tokens;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.UserRoles;
using BasvuruSistemi.Server.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TS.Result;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class TokenService(
    IUserTenantRoleRepository userTenantRoleRepository,
    IInvitationTokenRepository invitationTokenRepository,
    ICurrentUserService currentUserService,
    IEmailService emailService,
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    IAuthorizationService authorizationService
    ) : ITokenService
{
    public async Task<Result<string>> SendInvitationAsync(Guid inviteeId, Guid roleId, Guid? unitId, CancellationToken ct)
    {
        Guid? userId = currentUserService.UserId;
        if(!userId.HasValue)
            return Result<string>.Failure(404,"User not found.");

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure(404, "Tenant not found.");

        var res = await authorizationService.IsTenantManagerAsync(userId.Value, tenantId.Value, ct);
        if (!res)
            return Result<string>.Failure("You don't have the required role for invitation");

        var invitee = await userManager.FindByIdAsync(inviteeId.ToString());
        if(invitee is null || invitee.Email is null)
            return Result<string>.Failure(404, "Invitee not found.");

        var role = await roleManager.FindByIdAsync(roleId.ToString());
        if (role is null)
            return Result<string>.Failure(404, "Role not found.");

        var invitationToken = new InvitationToken(
            inviterId: userId.Value,
            inviteeId: invitee.Id,
            email: invitee.Email,
            token: Guid.CreateVersion7().ToString(),
            roleId: roleId,
            expiresAt: DateTimeOffset.Now.AddHours(1),
            tenantId: tenantId,
            unitId: unitId
        );

        invitationTokenRepository.Add(invitationToken);

        await unitOfWork.SaveChangesAsync(ct);

        var context = httpContextAccessor.HttpContext?.Request;

        var uiUrl = configuration["UI:Url"];

        var inviteLink = $"{uiUrl}/invitation?token={invitationToken.Token}";

        await emailService.SendAsync(
            invitee.Email,
            "Rol Daveti",
            $"<!DOCTYPE html>\r\n<html lang=\"tr\">\r\n<head>\r\n  <meta charset=\"UTF-8\" />\r\n  <title>Rol Daveti</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif; line-height: 1.6; color: #333;\">\r\n  <h2>Rol Davetiyeniz</h2>\r\n  <p>Merhaba,</p>\r\n  <p>\r\n    Size <strong>{role.Name}</strong> rolü atamak için bir davetiye gönderildi.  \r\n    Rol isteğinizi kabul etmek için lütfen aşağıdaki butona tıklayınız:\r\n  </p>\r\n  <p style=\"text-align: center; margin: 20px 0;\">\r\n    <a href=\"{inviteLink}\" \r\n       style=\"display: inline-block; padding: 10px 20px; background-color: #0078d4; color: #fff; text-decoration: none; border-radius: 4px;\"\r\n       target=\"_blank\">\r\n      Rol isteğini kabul et\r\n    </a>\r\n  </p>\r\n  <p>\r\n    Bu davet <strong>{DateTimeOffset.Now.AddHours(1)}</strong> tarihinde sona erecektir.  \r\n    Süre dolduğunda link geçersiz olacaktır.\r\n  </p>\r\n  <hr />\r\n  <p style=\"font-size: 0.9em; color: #666;\">\r\n   </p>\r\n</body>\r\n</html>",
            ct);

        return Result<string>.Succeed("Invitation sent successfully.");

    }

    public async Task<Result<string>> VerifyInvitationTokenAsync(string token, CancellationToken ct)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure("User not found");

        var invitationToken = await invitationTokenRepository.FirstOrDefaultAsync(p => p.Token == token, ct);
        if (invitationToken is null)
            return Result<string>.Failure("Token not found");

        if (invitationToken.IsUsed)
            return Result<string>.Failure("Token already used");

        if (invitationToken.ExpiresAt < DateTimeOffset.Now)
            return Result<string>.Failure("Token expired");

        if (invitationToken.InviteeId != userId)
            return Result<string>.Failure("You are not invited to this role");

        var roleExist = await userTenantRoleRepository.AnyAsync(p => p.UserId == invitationToken.InviteeId && p.RoleId == invitationToken.RoleId &&
                                                                    p.TenantId == invitationToken.TenantId &&
                                                                    p.UnitId == invitationToken.UnitId, ct);
        if(roleExist)
            return Result<string>.Failure("You already have this role");

        invitationToken.MarkAsUsed();
        invitationTokenRepository.Update(invitationToken);

        var userRole = new AppUserTenantRole(
            userId: invitationToken.InviteeId.Value,
            roleId: invitationToken.RoleId,
            tenantId: invitationToken.TenantId!.Value,
            unitId: invitationToken.UnitId
        );

        userTenantRoleRepository.Add(userRole);

        await unitOfWork.SaveChangesAsync(ct);
        return Result<string>.Succeed("Role assigned successfully");
    }
}
