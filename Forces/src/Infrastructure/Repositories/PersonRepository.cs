using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IRepositoryAsync<Person, int> _repository;

        public PersonRepository(IRepositoryAsync<Person, int> repository)
        {
            _repository = repository;
        }

        public async Task<Person> Add(Person Person)
        {
            return await _repository.AddAsync(Person);
        }

        public async Task<Person> Delete(Person Person)
        {
            await _repository.DeleteAsync(Person);

            return Person;
        }

        public Task<List<Person>> GetAll()
        {
            return _repository.GetAllAsync();
        }

        public Task<Person> GetById(int Id)
        {
            return _repository.GetByIdAsync(Id);
        }

        public async Task<Person> Update(Person Person)
        {
            await _repository.UpdateAsync(Person);

            return Person;
        }
    }
}
