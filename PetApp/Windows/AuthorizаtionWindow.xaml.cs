using PetApp.Data;
using PetApp.DbConnection;
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

namespace PetApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizаtionWindow.xaml
    /// </summary>
    public partial class AuthorizаtionWindow : Window
    {
        public AuthorizаtionWindow()
        {
            InitializeComponent();
        }

        private void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(UsernameTb.Text) || String.IsNullOrEmpty(ParolPb.Password))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            var user = DataBaseManager.AuthenticateUser(UsernameTb.Text, ParolPb.Password);

            if (user != null)
            {
                UserContext.AuthUser = user;
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password!");
            }
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(LoginTb.Text) || String.IsNullOrEmpty(PasswordPb.Password) || String.IsNullOrEmpty(ConfirmPasswordPb.Password))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            if (PasswordPb.Password != ConfirmPasswordPb.Password)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }


            if (!IsValidPassword(PasswordPb.Password))
            {
                MessageBox.Show("Invalid password format. It should be at least 6 characters long, contain at least one uppercase letter, one digit, and one special character.");
                return;
            }

            if (DataBaseManager.UserExists(LoginTb.Text))
            {
                MessageBox.Show("Username already exists!");
                return;
            }

            var newUser = new Users();
            newUser.username = LoginTb.Text;
            newUser.password = PasswordPb.Password;
            newUser.id_role = 2;
            DataBaseManager.AddUser(newUser);
            DataBaseManager.UpdateDatabase();
            if (DataBaseManager.AddUser(newUser))
            {
                MessageBox.Show("User registered successfully!");
                UserContext.AuthUser = newUser;
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to register user!");
            }
        }

        private bool Auth(string username, string password)
        {
            var user = DataBaseManager.GetUsers().FirstOrDefault(x => x.username == username && x.password == password);

            if (user != null)
            {
                UserContext.AuthUser = user;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SwitchToAuthBtn_Click(object sender, RoutedEventArgs e)
        {
            RegLbl.Visibility = Visibility.Collapsed;
            AuthLbl.Visibility = Visibility.Visible;
            RegisterStackPanel.Visibility = Visibility.Collapsed;
            AuthStackPanel.Visibility = Visibility.Visible;
            SwitchToAuthBtn.Visibility = Visibility.Collapsed;
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 6) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsDigit)) return false;
            if (!password.Any(ch => !char.IsLetterOrDigit(ch))) return false;
            return true;
        }
    }
}
