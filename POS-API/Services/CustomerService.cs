using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using POS_API.Helpers;

namespace POS_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerRepo.GetAllAsync();

            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Active = (c.DeletedAt == null),
                JoinDate = c.CreatedAt
            });
        }

        public async Task<PagedResponse<CustomerDto>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var (customers, totalRecords) = await _customerRepo.GetPagedAsync(pageNumber, pageSize, searchTerm);

            var customerDtos = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Active = (c.DeletedAt == null),
                JoinDate = c.CreatedAt
            });

            return new PagedResponse<CustomerDto>(customerDtos, totalRecords, pageNumber, pageSize);
        }

        public async Task<CustomerDto?> GetByIdAsync(long id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Active = (customer.DeletedAt == null),
                JoinDate = customer.CreatedAt
            };
        }

        public async Task<CustomerDto> CreateAsync(CustomerCreateDto customerDto)
        {

            if (!string.IsNullOrEmpty(customerDto.Email))
            {
                var existingCustomer = await _customerRepo.GetByEmailAsync(customerDto.Email);
                if (existingCustomer != null)
                {
                    throw new Exception($"Email '{customerDto.Email}' already exists.");
                }
            }

            var customerEntity = new Customer
            {
                FullName = customerDto.FullName,
                PhoneNumber = customerDto.PhoneNumber,
                Email = customerDto.Email,
                JoinDate = DateTime.Now
            };


            var newCustomer = await _customerRepo.CreateAsync(customerEntity);

            return new CustomerDto
            {
                Id = newCustomer.Id,
                FullName = newCustomer.FullName,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email,
                Active = true,
                JoinDate = newCustomer.JoinDate
            };

        }

        public async Task UpdateAsync(long id, CustomerUpdateDto customerDto)
        {
            var existingCustomer = await _customerRepo.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found");
            }

            existingCustomer.FullName = customerDto.FullName;
            existingCustomer.PhoneNumber = customerDto.PhoneNumber;
            existingCustomer.Email = customerDto.Email;

            if (customerDto.Active == true && existingCustomer.DeletedAt != null)
            {
                //mengaktifkan kembali customer yg ter-deaktivasi
                existingCustomer.DeletedAt = null;
            } else if (customerDto.Active == false && existingCustomer.DeletedAt == null)
            {
                // soft delete
                existingCustomer.DeletedAt = DateTime.Now;
            }

            await _customerRepo.UpdateAsync(existingCustomer);
        }

        public async Task DeleteAsync(long id)
        {
            var customerToDelete = await _customerRepo.GetByIdAsync(id);
            if (customerToDelete == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            await _customerRepo.DeleteAsync(customerToDelete);
        }

    }
}
