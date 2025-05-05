using BasvuruSistemi.Server.Domain.Employees;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;

internal sealed class EmployeeRepository : Repository<Employee, ApplicationDbContext>, IEmployeeRespository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }
}


