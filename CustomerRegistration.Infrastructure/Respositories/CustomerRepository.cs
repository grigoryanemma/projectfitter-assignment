using Microsoft.EntityFrameworkCore;

using CustomerRegistration.Application.Interfaces.Repository;
using CustomerRegistration.Domain.Entities;
using CustomerRegistration.Infrastructure.Data;

namespace CustomerRegistration.Infrastructure.Respositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _appDbContext;
        public CustomerRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Customer customer)
        {
            await _appDbContext.Customers.AddAsync(customer);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<Customer?> GetByIcNumber(string icNumber)
        {
            return await _appDbContext.Customers.FirstOrDefaultAsync(cus => cus.IcNumber == icNumber);
        }
    }
}
