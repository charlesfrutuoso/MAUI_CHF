using SQLite;
using TodoAppMaui.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoAppMaui.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TaskItem>().Wait(); // cria a tabela se não existir
        }

        // Inserir tarefa
        public Task<int> AddTaskAsync(TaskItem task)
        {
            return _database.InsertAsync(task);
        }

        // Atualizar tarefa
        public Task<int> UpdateTaskAsync(TaskItem task)
        {
            return _database.UpdateAsync(task);
        }

        // Listar todas as tarefas
        public Task<List<TaskItem>> GetTasksAsync()
        {
            return _database.Table<TaskItem>().ToListAsync();
        }

        // Excluir tarefa
        public Task<int> DeleteTaskAsync(TaskItem task)
        {
            return _database.DeleteAsync(task);
        }
    }
}
