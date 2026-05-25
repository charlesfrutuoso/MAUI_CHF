using SQLite;

namespace TodoAppMaui.Models
{
    public class TaskItem
    {
        [PrimaryKey, AutoIncrement]   // chave primária autoincremental
        public int Id { get; set; }

        [MaxLength(250)]              // limite de caracteres
        public string? Name { get; set; }

        public bool IsCompleted { get; set; }
    }
}
