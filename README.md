# How to create a MAUI Blazor Application (.NET9) for invoking an Azure Function with CRUD operations in a Azure CosmosDb

## 1. 

## 2. Create a new MAUI Blazor Application (.NET9) with Visual Studio 2022 Community Edition 



## 3. Add the Data Model

We create a Models folder and add the data model **TodoItem.cs** file

![image](https://github.com/user-attachments/assets/ab92605f-5da8-4a78-b976-9563bdaefe5f)

See the TodoItem.cs file source code:

```csharp
namespace BlazorWebAssemblyForInvokingAzureFunctionCosmosDBCRUD.Models
{
    public class TodoItem
    {
        public string? id { get; set; }  // ID único del ítem
        public string? name { get; set; }  // Nombre del ítem
        public string? description { get; set; }  // Descripción del ítem
        public bool IsCompleted { get; set; }  // Estado de completado (si es necesario)
        public string? _rid { get; set; }  // Metadatos específicos de Cosmos DB
        public string? _self { get; set; }
        public string? _etag { get; set; }
        public string? _attachments { get; set; }
        public long? _ts { get; set; }  // Timestamp de creación o actualización
    }
}
```

## 4. Add the Service for invoking the Azure Function

Add the Services 

![image](https://github.com/user-attachments/assets/5f215aa9-093b-417e-a748-9fe503810301)

See the **CosmosDbService.cs** source code:

```csharp
using BlazorWebAssemblyForInvokingAzureFunctionCosmosDBCRUD.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlazorWebAssemblyForInvokingAzureFunctionCosmosDBCRUD.Services
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
```

**IMPORTANT NOTE**: copy from Azure Function the Authorization code

We copy the Azure Function URL

![image](https://github.com/user-attachments/assets/b6bdda60-0947-4273-b9ed-9e50c16b6e26)

We copy from the **code** query string the Authorization code:

https://mycosmosdbcrudazurefunction.azurewebsites.net/api/items/{id?}?code=SWCyUgbqY6yDdAMhmP69EekVukGjwhPl2oQWWWMoUDK-AzFucN7Zjw%3D%3D

We paste the authorization code in the razor component source code to include in the Azure Function Call

```csharp
private const string FunctionAuthorizationCode = "SWCyUgbqY6yDdAMhmP69EekVukGjwhPl2oQWWWMoUDK-AzFucN7Zjw%3D%3D";
```

## 5. Add the razor Component



## 6. Modify the middleware (Program.cs)





## 7. Run the application and see the result
