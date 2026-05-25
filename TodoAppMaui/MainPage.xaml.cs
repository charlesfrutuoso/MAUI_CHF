using TodoAppMaui.Models;
using System.Collections.ObjectModel;

namespace TodoAppMaui
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<TaskItem> _pendingTasks;
        private ObservableCollection<TaskItem> _completedTasks;

        public MainPage()
        {
            InitializeComponent();
            _pendingTasks = new ObservableCollection<TaskItem>();
            _completedTasks = new ObservableCollection<TaskItem>();

            pendingTasksCollection.ItemsSource = _pendingTasks;
            completedTasksCollection.ItemsSource = _completedTasks;
        }

        // Evento do botão "Adicionar"
        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            string taskName = taskEntry.Text;

            if (!string.IsNullOrWhiteSpace(taskName))
            {
                var newTask = new TaskItem { Name = taskName };

                // Salva no banco
                await App.Database.AddTaskAsync(newTask);

                // Atualiza a lista
                await LoadTasksAsync();

                // Limpa o campo
                taskEntry.Text = string.Empty;
            }
        }

        // Carrega tarefas do banco
        private async Task LoadTasksAsync()
        {
            var tasksFromDb = await App.Database.GetTasksAsync();

            _pendingTasks.Clear();
            _completedTasks.Clear();

            foreach (var task in tasksFromDb)
            {
                if (task.IsCompleted)
                    _completedTasks.Add(task);
                else
                    _pendingTasks.Add(task);
            }
        }

        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem taskToDelete)
            {
                // Remove do banco
                await App.Database.DeleteTaskAsync(taskToDelete);

                // Atualiza a lista
                await LoadTasksAsync();
            }
        }

        private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is TaskItem selectedTask)
            {
                // Prompt para editar
                string newName = await DisplayPromptAsync(
                    "Editar Tarefa",
                    "Digite o novo nome:",
                    initialValue: selectedTask.Name);

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    selectedTask.Name = newName;

                    // Atualiza no banco
                    await App.Database.UpdateTaskAsync(selectedTask);

                    // Recarrega lista
                    await LoadTasksAsync();
                }

                // limpar seleção da CollectionView que disparou o evento
                if (sender is CollectionView collectionView)
                {
                    collectionView.SelectedItem = null;
                }
            }
        }

        private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.BindingContext is TaskItem task)
            {
                task.IsCompleted = e.Value;

                // Atualiza no banco
                await App.Database.UpdateTaskAsync(task);

                // Opcional: recarregar lista
                await LoadTasksAsync();
            }
        }

        // Quando a página aparece, carregamos as tarefas
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTasksAsync();
        }
    }
}
