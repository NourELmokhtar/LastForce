using Forces.Application.Features.Person.Commands.AddEdit;
using Forces.Application.Features.Person.Queries.GetAll;
using Forces.Application.Features.Person.Queries.GetBySpecifics;
using Forces.Client.Infrastructure.Extensions;
using Forces.Client.Infrastructure.Managers.Person;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Person
{
    public class PersonManager : IPersonManager
    {
        private protected readonly HttpClient _httpClient;

        public PersonManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.PersonEndPoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllPersonsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PersonEndPoints.GetAll);
            return await response.ToResult<List<GetAllPersonsResponse>>();
        }

        public async Task<IResult<GetPersonByResponse>> GetPersonByAsync(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.PersonEndPoints.GetPersonById(Id));
            return await response.ToResult<GetPersonByResponse>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditPersonCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonEndPoints.Save, request);
            return await response.ToResult<int>();
        }

    }

}
