using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskHelper.Models;

namespace TaskHelper
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        
        public MenuWindow()
        {
            InitializeComponent();
            LoadData();
            User currentUser;                        
            currentUser = Helper.userSession;
            DataContext = currentUser;
           
        }
        private void LoadData()
        {
            UserTasksDGrid.ItemsSource = Helper.db.Tasks.Include(q => q.StatusTask).Include(w => w.Creator).Include(a => a.Acceptor).Where(x => x.CreatorId == Helper.userSession.UserId).ToList();
            ClosedTasksDGrid.ItemsSource = Helper.db.Tasks.Include(q => q.StatusTask).Include(w => w.Creator).Include(a => a.Acceptor).Where(x => x.AcceptorId == Helper.userSession.UserId).Where(x => x.StatusTaskId == 3).ToList();
            OpenTasksDGrid.ItemsSource = Helper.db.Tasks.Include(q => q.StatusTask).Include(w => w.Creator).Include(a => a.Acceptor).Where(x => x.StatusTaskId == 1).ToList();
            UsersDGrid.ItemsSource = Helper.db.Users.ToList();
        }
        private void UsersDGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = UsersDGrid.SelectedItem as User;
            if (user != null)
            {
                Helper.db.SaveChanges();
                LoadData();
            }
        }
        private void UsersTaskSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = Helper.db.Users.FirstOrDefault(q => q.Name == SearchBox.Text);
            if (user != null)
            {
                Models.Task task = Helper.db.Tasks.FirstOrDefault(q => q.CreatorId == user.UserId);
                new TakeTaskWindow(task).Show();
                this.Close();
            }


        }
        private void OpenTasksDGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Models.Task selectedTask = OpenTasksDGrid.SelectedItem as Models.Task;

            Helper.task = selectedTask;

            if (selectedTask != null)
            {
                new TakeTaskWindow(selectedTask).Show();
                this.Close();
            }

        }
        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void ChangeTaskStatusBtn_Click(object sender, RoutedEventArgs e)
        {
            Models.Task selectedTask = UserTasksDGrid.SelectedItem as Models.Task;

            Models.Task task = Helper.db.Tasks.First(q => q.TaskId == selectedTask.TaskId);

            if (selectedTask.StatusTaskId == 1 || selectedTask.StatusTaskId == 2)
            {
                task.StatusTaskId++;
                Helper.db.SaveChanges();
            }
            else if (selectedTask.StatusTaskId == 3)
            {
                task.StatusTaskId = 1;
                Helper.db.SaveChanges();
            }

            LoadData();
        }

        private void CreateTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            new TaskCreateWindow().Show();
            this.Close();
        }
    }
}
