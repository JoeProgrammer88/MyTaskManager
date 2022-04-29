using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MyTaskManager.Data;
using MyTaskManager.Models;
using System.Runtime.InteropServices;

namespace MyTaskManager.Pages
{
    public partial class Index
    {
        private bool _isDataSaved = true;

        private List<ToDoItem> todos = new List<ToDoItem>();

        private string _newTitle = null!;

        [Inject]
        private IJSRuntime _js { get; set; }

        [Inject]
        private IDbContextFactory<TaskManagerContext> _dbContextFactory { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
            {
                // Create SQLite file in browser
                var module = await _js.InvokeAsync<IJSObjectReference>("import", "./SqliteBrowserConnector.js");
                await module.InvokeVoidAsync("synchronizeFileWithIndexedDb", "MyTaskManager.db");
            }

            using TaskManagerContext db = await _dbContextFactory.CreateDbContextAsync();
            await db.Database.EnsureCreatedAsync();

            if (!db.ToDos.Any())
            {
                await db.ToDos.AddRangeAsync(
                    new ToDoItem { Title = "First Task" },
                    new ToDoItem { Title = "Second Task" },
                    new ToDoItem { Title = "Third Task" }
                );

                await db.SaveChangesAsync();
            }

            todos = await db.ToDos.ToListAsync();

            await base.OnInitializedAsync();
        }

        private async Task AddTask()
        {
            // Add to Database
            using TaskManagerContext db = await _dbContextFactory.CreateDbContextAsync();
            ToDoItem item = new ToDoItem { Title = _newTitle };
            await db.ToDos.AddAsync(item);
            await db.SaveChangesAsync();

            // Add to UI
            todos.Add(item);

            _isDataSaved = false;
        }

        private async Task DeleteTask(ToDoItem item)
        {
            using TaskManagerContext db = await _dbContextFactory.CreateDbContextAsync();

            // Remove from Database
            db.ToDos.Remove(item);
            await db.SaveChangesAsync();

            // Remove from UI
            todos.Remove(item);

            _isDataSaved = false;
        }

        private async Task UpdateTask(ToDoItem item)
        {
            using TaskManagerContext db = await _dbContextFactory.CreateDbContextAsync();
            db.ToDos.Update(item);
            await db.SaveChangesAsync();

            _isDataSaved = false;
        }

        private async Task SaveData()
        {
            var module = await _js.InvokeAsync<IJSObjectReference>("import", "./SqliteBrowserConnector.js");
            await module.InvokeVoidAsync("saveDatabaseToBrowser", "MyTaskManager.db");
            _isDataSaved = true;
        }
    }
}
