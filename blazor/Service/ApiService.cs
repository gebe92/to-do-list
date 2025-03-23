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

        public async Task<List<TaskList>> GetByUserAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/todo/user/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }

            var taskList = await response.Content.ReadFromJsonAsync<List<TaskList>>();

            if (taskList == null)
            {
                throw new InvalidOperationException("Received null response content while fetching the task list.");
            }

            return taskList;
        }

        public async Task<TaskList> GetByIdAsync(int taskId)
        {
            var response = await _httpClient.GetAsync($"api/todo/{taskId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }

            var taskList = await response.Content.ReadFromJsonAsync<TaskList>();

            if (taskList == null)
            {
                throw new InvalidOperationException("Received null response content while fetching the task list.");
            }

            return taskList;
        }

        public async Task<TaskList> UpdateAsync(int taskId, UpdateTaskDto taskDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/todo/{taskId}", taskDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }

            var taskList = await response.Content.ReadFromJsonAsync<TaskList>();

            if (taskList == null)
            {
                throw new InvalidOperationException("Received null response content while fetching the task list.");
            }

            return taskList;
        }

        public async Task<bool> DeleteAsync(int taskId)
        {
            var response = await _httpClient.DeleteAsync($"api/todo/{taskId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }

            return true;
        }
    }
}
