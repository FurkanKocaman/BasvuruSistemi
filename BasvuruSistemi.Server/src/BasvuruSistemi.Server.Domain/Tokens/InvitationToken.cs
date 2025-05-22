using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Roles;
using BasvuruSistemi.Server.Domain.Units;
using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.Tokens;
public sealed class InvitationToken : Entity
{
    public Guid InviterId { get; private set; }
    public AppUser Inviter { get; private set; } = default!;
    public Guid? InviteeId { get; private set; }
    public AppUser? Invitee { get; private set; }
    public string Email { get; private set; } = default!;
    public string Token { get; private set; } = default!;
    public Guid RoleId { get; private set; } = default!;
    public AppRole Role { get; private set; } = default!;
    public Guid? TenantId { get; private set; }
    public Guid? UnitId { get; private set; }
    public Unit? Unit { get; private set; }

    public DateTimeOffset ExpiresAt { get; private set; }

    public bool IsUsed { get; private set; }
    public DateTimeOffset? UsedAt { get; private set; }

    private InvitationToken() { }

    public InvitationToken(
        Guid inviterId,
        Guid inviteeId,
        string email,
        string token,
        Guid roleId,
        DateTimeOffset expiresAt,
        Guid? tenantId = null,
        Guid? unitId = null)
    {
        InviterId = inviterId;
        InviteeId = inviteeId;
        Email = email;
        Token = token;
        RoleId = roleId;
        TenantId = tenantId;
        UnitId = unitId;
        CreatedAt = DateTimeOffset.Now;
        ExpiresAt = expiresAt;
        IsUsed = false;
    }

    public void MarkAsUsed()
    {
        if (IsUsed) throw new InvalidOperationException("Token already used.");
        IsUsed = true;
        UsedAt = DateTimeOffset.Now;
    }
}
