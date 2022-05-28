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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskHelper.Models;

namespace TaskHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AuthentificationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "" || PasswordBox.Password == "")
            {
                MessageBox.Show("Пожалуйста, введите логин или пароль!", "Ошибка");
            }
            else
            {
                User user = Helper.db.Users.FirstOrDefault(q => q.Login == LoginBox.Text && q.Password == PasswordBox.Password);
                if (user != null)
                {
                    Helper.userSession = user;
                    Helper.db.SaveChanges();
                    new MenuWindow().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }
            }
        }
        private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            new RegistrationWindow().Show();
            this.Close();
        }
    }
}
