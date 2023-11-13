using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetAll();
        Task<Person> GetById(int Id);
        Task<Person> Add(Person Person);
        Task<Person> Update(Person Person);
        Task<Person> Delete(Person Person);
    }
}
