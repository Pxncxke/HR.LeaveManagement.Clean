using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        public LeaveAllocationRepository(HrDatabaseContext context) : base(context)
        {

        }

        public async Task AddAllocations(List<LeaveAllocation> allocations)
        {
            await context.AddRangeAsync(allocations);
        }

        public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
        {
            return await context.LeaveAllocations.AnyAsync(w => w.EmployeeId == userId 
                                && w.LeaveTypeId == leaveTypeId 
                                && w.Period == period);
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            return await context.LeaveAllocations.Include(w => w.LeaveType).FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationWithDetails()
        {
            return await context.LeaveAllocations.Include(w => w.LeaveType).ToListAsync();
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationWithDetails(string userId)
        {
            return await context.LeaveAllocations.Include(w => w.LeaveType).Where(w => w.EmployeeId == userId).ToListAsync();
        }

        public async Task<LeaveAllocation> GetUserLeaveAllocations(string userId, int leaveTypeId)
        {
            return await context.LeaveAllocations
                .FirstOrDefaultAsync(w => w.EmployeeId == userId && w.LeaveTypeId == leaveTypeId);
        }
    }
}
