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

namespace TaskHelper
{
    /// <summary>
    /// Логика взаимодействия для TakeTaskWindow.xaml
    /// </summary>
    public partial class TakeTaskWindow : Window
    {
        
        private Models.Task selectedTask;
        public TakeTaskWindow(Models.Task selectedTask)
        {
            InitializeComponent();
            this.selectedTask = Helper.task;
            DataContext = selectedTask;
            LoadData();
        }

        private void LoadData()
        {
          
        }

        private void TakeTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            Models.Task selectedTask = Helper.task;

            selectedTask.StatusTaskId = 2;
            selectedTask.AcceptorId = Helper.userSession.UserId;
            Helper.db.SaveChanges();
            MessageBox.Show("Задание взято!");
            new MenuWindow().Show();
            this.Close();
        }
    }
}
