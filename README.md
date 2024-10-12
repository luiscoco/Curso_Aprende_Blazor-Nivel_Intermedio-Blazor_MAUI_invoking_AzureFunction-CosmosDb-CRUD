# How to create a MAUI Blazor Application (.NET9) for invoking an Azure Function with CRUD operations in a Azure CosmosDb

## 1. Create an Azure CosmosDb database and an Azure Function for CRUD operations over the referred database

See the detailed explanation in this publication URL: https://github.com/luiscoco/MicroServices-AzureFunction_CosmosDB-CRUD

## 2. Create a new MAUI Blazor Application (.NET9) with Visual Studio 2022 Community Edition 

Install and Run Visual Studio 2022 and create a new project

![image](https://github.com/user-attachments/assets/81606aa4-2f68-4c80-bea6-7c50573deec8)

Select the MAUI Blazor project template

![image](https://github.com/user-attachments/assets/d368058a-3d2a-4286-8154-e452dda3599c)

Input the project name and location

![image](https://github.com/user-attachments/assets/61ac08e2-05e8-4792-89f0-e1e618dbd66a)

Select the **.NET9** Fraemwork and press the create button

![image](https://github.com/user-attachments/assets/3782ec4c-50ab-4600-acd3-01bc0966e5dc)

This is the project folders and files structure. We added the **Models** and **Services** new folders

![image](https://github.com/user-attachments/assets/b2d5d4b3-8e3d-4334-b34f-90e24d33d160)

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

**IMPORTANT NOTE**: copy from Azure **FunctionAuthorizationCode**

We copy the Azure Function URL

![image](https://github.com/user-attachments/assets/b6bdda60-0947-4273-b9ed-9e50c16b6e26)

We copy from the **code** query string the Authorization code:

https://mycosmosdbcrudazurefunction.azurewebsites.net/api/items/{id?}?code=SWCyUgbqY6yDdAMhmP69EekVukGjwhPl2oQWWWMoUDK-AzFucN7Zjw%3D%3D

We paste the authorization code in the razor component source code to include in the Azure Function Call

```csharp
private const string FunctionAuthorizationCode = "SWCyUgbqY6yDdAMhmP69EekVukGjwhPl2oQWWWMoUDK-AzFucN7Zjw%3D%3D";
```

## 5. Add the razor Component

We right click on the Pages folder and select the menu option Add->Razor Component...

![image](https://github.com/user-attachments/assets/1967f380-1f7e-411a-8ecb-9e37f085581b)

We input the new component code

```razor
@page "/manage-items"
@using BlazorWebAssemblyForInvokingAzureFunctionCosmosDBCRUD.Models
@using BlazorWebAssemblyForInvokingAzureFunctionCosmosDBCRUD.Services
@inject CosmosDbService CosmosService

<div class="container mt-5">
    <h3 class="text-primary mb-4">To-Do List and Item Management</h3>

    <!-- Section to create a new item -->
    <div class="card mb-4 shadow-sm">
        <div class="card-body">
            <h4 class="card-title text-secondary">Create a new To-Do item</h4>

            <EditForm Model="newItem" OnValidSubmit="CreateNewItem">
                <DataAnnotationsValidator />
                <ValidationSummary class="text-danger" />

                <div class="mb-3">
                    <label class="form-label">Name:</label>
                    <InputText @bind-Value="newItem.name" class="form-control" />
                </div>
                <div class="mb-3">
                    <label class="form-label">Description:</label>
                    <InputText @bind-Value="newItem.description" class="form-control" />
                </div>
                <div class="form-check mb-3">
                    <InputCheckbox @bind-Value="newItem.IsCompleted" class="form-check-input" />
                    <label class="form-check-label">Completed</label>
                </div>

                <button type="submit" class="btn btn-success">Create Item</button>
            </EditForm>

            @if (createMessage != null)
            {
                <p class="mt-3 alert alert-info">@createMessage</p>
            }
        </div>
    </div>

    <!-- Section to list existing items -->
    <div class="card mb-4 shadow-sm">
        <div class="card-body">
            <h4 class="card-title text-secondary">Existing To-Do Items</h4>

            @if (items == null)
            {
                <p><em>Loading...</em></p>
            }
            else if (!items.Any())
            {
                <p class="text-warning">No items found.</p>
            }
            else
            {
                <ul class="list-group">
                    @foreach (var item in items)
                    {
                        <li class="list-group-item d-flex justify-content-between align-items-center">
                            <div>
                                <strong>@item.name</strong> - @item.description -
                                <span class="badge bg-primary">@((item.IsCompleted) ? "Completed" : "Pending")</span>
                            </div>
                            <div>
                                <button class="btn btn-danger btn-sm me-2" @onclick="() => DeleteItem(item.id)">Delete</button>
                                <button class="btn btn-warning btn-sm" @onclick="() => EditItem(item.id)">Update</button>
                            </div>
                        </li>
                    }
                </ul>
            }
        </div>
    </div>

    <!-- Section to update existing item -->
    @if (isEditMode)
    {
        <div class="card mb-4 shadow-sm">
            <div class="card-body">
                <h4 class="card-title text-secondary">Update To-Do Item</h4>

                @if (currentItem != null)
                {
                    <EditForm Model="currentItem" OnValidSubmit="UpdateItem">
                        <DataAnnotationsValidator />
                        <ValidationSummary class="text-danger" />

                        <div class="mb-3">
                            <label class="form-label">Name:</label>
                            <InputText @bind-Value="currentItem.name" class="form-control" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Description:</label>
                            <InputText @bind-Value="currentItem.description" class="form-control" />
                        </div>
                        <div class="form-check mb-3">
                            <InputCheckbox @bind-Value="currentItem.IsCompleted" class="form-check-input" />
                            <label class="form-check-label">Completed</label>
                        </div>

                        <button type="submit" class="btn btn-primary">Update Item</button>
                        <button type="button" class="btn btn-secondary" @onclick="CancelEdit">Cancel</button>
                    </EditForm>

                    @if (updateMessage != null)
                    {
                        <p class="mt-3 alert alert-info">@updateMessage</p>
                    }
                }
            </div>
        </div>
    }
</div>

@code {
    private List<TodoItem> items;
    private TodoItem newItem = new TodoItem();
    private TodoItem currentItem = null; // Holds the item being updated
    private string createMessage;
    private string updateMessage;
    private bool isEditMode = false; // Flag to show or hide the update form

    protected override async Task OnInitializedAsync()
    {
        // Load existing items when the page initializes
        items = await CosmosService.GetItemsAsync();
    }

    // Create a new item
    private async Task CreateNewItem()
    {
        newItem.id = Guid.NewGuid().ToString(); // Generate a new unique ID for the item
        bool isCreated = await CosmosService.CreateItemAsync(newItem);
        if (isCreated)
        {
            createMessage = "Item created successfully!";
            newItem = new TodoItem(); // Reset the form after success
            items = await CosmosService.GetItemsAsync(); // Refresh the list after creating a new item
        }
        else
        {
            createMessage = "Failed to create item.";
        }
    }

    // Delete an item by ID
    private async Task DeleteItem(string id)
    {
        bool isDeleted = await CosmosService.DeleteItemAsync(id);
        if (isDeleted)
        {
            items = await CosmosService.GetItemsAsync(); // Refresh the list after deletion
        }
    }

    // Enter edit mode for an item by ID
    private async Task EditItem(string id)
    {
        currentItem = await CosmosService.GetItemByIdAsync(id);
        if (currentItem != null)
        {
            isEditMode = true; // Show the update form
            updateMessage = null; // Clear any previous update messages
        }
    }

    // Update the selected item
    private async Task UpdateItem()
    {
        bool isUpdated = await CosmosService.UpdateItemAsync(currentItem.id, currentItem);
        if (isUpdated)
        {
            updateMessage = "Item updated successfully!";
            items = await CosmosService.GetItemsAsync(); // Refresh the list after the update
            isEditMode = false; // Exit edit mode
        }
        else
        {
            updateMessage = "Failed to update item.";
        }
    }

    // Cancel editing
    private void CancelEdit()
    {
        currentItem = null; // Clear the current item
        isEditMode = false; // Hide the update form
    }
}
```

## 6. Modify the middleware (Program.cs)

We set the Azure Function base URL

```csharp
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://mycosmosdbcrudazurefunction.azurewebsites.net/") });
```

We register the new service adding this code in the middleware

```csharp
builder.Services.AddScoped<CosmosDbService>(); 
```

This is the whole middleware (Program.cs) code:

```csharp
using BlazorWebAssemblyForInvokingAzureFunctionCosmosDBCRUD;
using BlazorWebAssemblyForInvokingAzureFunctionCosmosDBCRUD.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://mycosmosdbcrudazurefunction.azurewebsites.net/") });
builder.Services.AddScoped<CosmosDbService>(); 

await builder.Build().RunAsync();
```

## 7. Run the application and see the result

### 7.1. Running the application as Windows Desketop

![image](https://github.com/user-attachments/assets/35d8e7e8-f4f3-4070-8d09-2377b1041f93)

### 7.2. Running the application in the Mobile Phone

Prior to run your application in your Mobile you have to connect the Mobile to your Laptop with a USB wire

Also in Visual Studio 2022 Community Edition you have to navigate to the menu option **Compile->Configurations Manager...** and select the checkbox **Implement**

![image](https://github.com/user-attachments/assets/c7d5df04-880f-4a8c-8e49-1ddc507f84c2)

![image](https://github.com/user-attachments/assets/f663aa50-bf93-4b86-8baf-7af7270aa8ca)

Now you have to check your mobile is connected to your laptop running this command:

```
adb devices
```

![image](https://github.com/user-attachments/assets/c61fb3f1-0559-4068-af74-77a69b343a64)

Then you have to select your Mobile in the **Android Local Devices** menu option 

![image](https://github.com/user-attachments/assets/e9759283-fb5a-4d8b-b4bf-ecca94c5cf63)

Then press the run button 

![image](https://github.com/user-attachments/assets/f4d03226-af16-4a3f-b93b-2f70fc75065d)

And see your MAUI application running in your Mobile and also in Visual Studio IDE

![image](https://github.com/user-attachments/assets/d9c374c2-7f75-48a5-addd-ed3fdfd0af3c)

![image](https://github.com/user-attachments/assets/19fa5ddb-79b7-4c62-aa92-2fab0ee92d15)
