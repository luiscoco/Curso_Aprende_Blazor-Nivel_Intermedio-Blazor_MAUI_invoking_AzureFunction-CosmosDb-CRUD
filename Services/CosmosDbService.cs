using MAUIBlazorAzureFunctionCRUDCosmosDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MAUIBlazorAzureFunctionCRUDCosmosDB.Services
{
    public class CosmosDbService
    {
        private readonly HttpClient _httpClient;
        private const string FunctionAuthorizationCode = "SWCyUgbqY6yDdAMhmP69EekVukGjwhPl2oQWWWMoUDK-AzFucN7Zjw%3D%3D";

        public CosmosDbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: List all items
        public async Task<List<TodoItem>> GetItemsAsync()
        {
            // Append the authorization code
            return await _httpClient.GetFromJsonAsync<List<TodoItem>>($"api/items?code={FunctionAuthorizationCode}");
        }

        // POST: Create a new item
        public async Task<bool> CreateItemAsync(TodoItem newItem)
        {
            // Append the authorization code
            var response = await _httpClient.PostAsJsonAsync($"api/items?code={FunctionAuthorizationCode}", newItem);
            return response.IsSuccessStatusCode;
        }

        // GET: Get an item by ID
        public async Task<TodoItem> GetItemByIdAsync(string id)
        {
            // Append the authorization code
            return await _httpClient.GetFromJsonAsync<TodoItem>($"api/items/{id}?code={FunctionAuthorizationCode}");
        }

        // PUT: Update an item by ID
        public async Task<bool> UpdateItemAsync(string id, TodoItem updatedItem)
        {
            // Append the authorization code
            var response = await _httpClient.PutAsJsonAsync($"api/items/{id}?code={FunctionAuthorizationCode}", updatedItem);
            return response.IsSuccessStatusCode;
        }

        // DELETE: Delete an item by ID
        public async Task<bool> DeleteItemAsync(string id)
        {
            // Append the authorization code
            var response = await _httpClient.DeleteAsync($"api/items/{id}?code={FunctionAuthorizationCode}");
            return response.IsSuccessStatusCode;
        }
    }



}
