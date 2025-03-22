using blazor.Models;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace blazor.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(CreateTaskDto task)
        {
            task.UserId = 1;
            task.Status = "Test";
            task.CreatedAt = DateTime.Now;

            var response = await _httpClient.PostAsJsonAsync("api/todo/create", task);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<List<TaskList>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskList>>("api/todo/getall");
        }
    }
}
