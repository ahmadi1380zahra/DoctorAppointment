using DoctorAppointment.Contracts.Interfaces;
using DoctorAppointment.Persistence.EF;

namespace DoctorAppointment.Persistance.EF;

public class EFUnitOfWork : UnitOfWork
{
    private readonly EFDataContext _context;

    public EFUnitOfWork(EFDataContext context)
    {
        _context = context;
    }

    public async Task Begin()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task Complete()
    {
        await _context.SaveChangesAsync();
    }

    public async Task RollBack()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}