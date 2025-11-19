using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;

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

            // LOGIKA MAPPING BARU
            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Active = (c.DeletedAt == null), // Hitung 'Active'
                JoinDate = c.CreatedAt // Mapping 'JoinDate'
            });
        }

        public async Task<CustomerDto?> GetByIdAsync(long id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null) return null;

            // LOGIKA MAPPING BARU
            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Active = (customer.DeletedAt == null), // Hitung 'Active'
                JoinDate = customer.CreatedAt // Mapping 'JoinDate'
            };
        }

        public async Task<CustomerDto> CreateAsync(CustomerCreateDto customerDto)
        {
            // Validasi email (tidak berubah)
            if (!string.IsNullOrEmpty(customerDto.Email))
            {
                var existingCustomer = await _customerRepo.GetByEmailAsync(customerDto.Email);
                if (existingCustomer != null)
                {
                    throw new Exception($"Email '{customerDto.Email}' already exists.");
                }
            }

            // LOGIKA MAPPING BARU: Lebih simpel!
            // Kita tidak perlu set 'Active' atau 'JoinDate'
            var customerEntity = new Customer
            {
                FullName = customerDto.FullName,
                PhoneNumber = customerDto.PhoneNumber,
                Email = customerDto.Email
            };

            var newCustomer = await _customerRepo.CreateAsync(customerEntity);

            // Mapping DTO untuk response (sama seperti GetById)
            return new CustomerDto
            {
                Id = newCustomer.Id,
                FullName = newCustomer.FullName,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email,
                Active = (newCustomer.DeletedAt == null),
                JoinDate = newCustomer.CreatedAt
            };
        }

        public async Task UpdateAsync(long id, CustomerUpdateDto customerDto)
        {
            // GetByIdAsync sekarang hanya mengambil customer yang aktif
            var existingCustomer = await _customerRepo.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            // Map field standar
            existingCustomer.FullName = customerDto.FullName;
            existingCustomer.PhoneNumber = customerDto.PhoneNumber;
            existingCustomer.Email = customerDto.Email;

            // LOGIKA BARU: Menerjemahkan bool 'Active' ke 'DeletedAt'
            if (customerDto.Active == true && existingCustomer.DeletedAt != null)
            {
                // Kasus: Meng-aktifkan kembali customer yang ter-deaktivasi
                existingCustomer.DeletedAt = null;
            }
            else if (customerDto.Active == false && existingCustomer.DeletedAt == null)
            {
                // Kasus: Men-deaktivasi customer (Soft Delete)
                existingCustomer.DeletedAt = DateTime.Now;
            }

            // Repository akan menangani 'UpdatedAt'
            await _customerRepo.UpdateAsync(existingCustomer);
        }

        public async Task DeleteAsync(long id)
        {
            // Service tidak berubah, tapi repository AKAN soft-delete
            var customerToDelete = await _customerRepo.GetByIdAsync(id);
            if (customerToDelete == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }
            await _customerRepo.DeleteAsync(customerToDelete);
        }
    }
}