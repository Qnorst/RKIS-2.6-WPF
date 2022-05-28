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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }
        private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameBox.Text.Trim();
            string SecondName = SecondNameBox.Text.Trim();
            string Patronymic = PatronymicBox.Text.Trim();
            string Login = LoginBox.Text.Trim();
            string Password = PasswordBox.Text.Trim();
            string PhoneNumber = PhoneNumberBox.Text.Trim();

                User user = new User()
                {
                    Login = Login,
                    Password = Password,
                    Name = Name,
                    SecondName = SecondName,
                    Patronymic = Patronymic,
                    PhoneNumber = PhoneNumber,
                };
                Helper.db.Users.Add(user);
                Helper.db.SaveChanges();

                MessageBox.Show("Регистрация успешна");

                new MainWindow().Show();
                this.Close();
        }
    }
}
