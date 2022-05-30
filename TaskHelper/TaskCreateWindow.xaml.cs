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
    /// Логика взаимодействия для TaskCreateWindow.xaml
    /// </summary>
    public partial class TaskCreateWindow : Window
    {
        public TaskCreateWindow()
        {
            InitializeComponent();
            User currentUser;
            currentUser = Helper.userSession;
            DataContext = currentUser;
        }

        private void CreateTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameBox.Text.Trim();
            string Describtion = TaskDescriptionBox.Text.Trim();
            int CreatorId = Helper.userSession.UserId;
            int AcceptorId = Helper.userSession.UserId;
            var PublicDate = DateTime.Now;
            int StatusTaskId = 1;

            Models.Task task = new Models.Task()
            {
                Name = Name,
                Describtion = Describtion,                
                PublicDate = PublicDate,
                CreatorId = CreatorId,
                AcceptorId = AcceptorId,
                StatusTaskId = StatusTaskId,
            };
            Helper.db.Tasks.Add(task);
            Helper.db.SaveChanges();

            MessageBox.Show("Задача создана!");

            new MenuWindow().Show();
            this.Close();
        }
    }
}
