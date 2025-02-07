using CustomerRegistration.Domain.Entities;

namespace CustomerRegistration.Application.Interfaces.Repository
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task SaveChangesAsync();
        Task<Customer?> GetByIcNumber(string icNumber);
    }
}
