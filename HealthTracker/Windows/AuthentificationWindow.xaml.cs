using HealthTracker.DatabaseContext;
using HealthTracker.Entities;
using HealthTracker.HashingData;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HealthTracker.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthentificationWindow.xaml
    /// </summary>
    public partial class AuthentificationWindow : Window
    {
        public AuthentificationWindow()
        {
            InitializeComponent();
        }

        private void ButtonMinimaze_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NavigationToMainMenu()
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Close();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActionSelectionCheckBox.IsChecked.Value)
            {
                bool registrationResult = Registration(LoginTextBox.Text, PasswordTextBox.Password);
                if (registrationResult)
                {
                    NavigationToMainMenu();
                }
            }
            else
            {
                bool authenticationResult = Authorization(LoginTextBox.Text, PasswordTextBox.Password);

                if (authenticationResult)
                {
                    NavigationToMainMenu();
                }
            }
        }

        public bool Registration(string login, string password)
        {
            if (new[] { login, password }.Any(x => String.IsNullOrWhiteSpace(x)))
            {
                MessageBox.Show("Проверьте заполненность полей!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (password.Length < 8 || password.Length > 20)
            {
                MessageBox.Show("Минимальная длина пароля - 8 символов, а максимальная длина - 20 символов", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (UserExists(login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            RegisterUser(new Users { Login = login, Password = HashingData.HashingPassword.HashPassword(password) });
            MessageBox.Show("Вы успешно зарегистрировались", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);

            return true;

        }

        public bool UserExists(string login)
        {
            return DBContext.Context.Users.FirstOrDefault(x => x.Login == login) != null;
        }

        private void RegisterUser(Users user)
        {
            try
            {
                DBContext.Context.Users.Add(user);
                DBContext.Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(eve => eve.ValidationErrors)
                    .Select(ve => ve.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                MessageBox.Show($"Ошибка при валидации: {fullErrorMessage}");
            }
        }

        public bool Authorization(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Проверьте заполненность полей!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            var user = DBContext.Context.Users.FirstOrDefault(x => x.Login == login);

            if (user == null)
            {
                MessageBox.Show("Пользователь не найден, Вам нужно пройти регистрацию", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (HashingPassword.VerifyPassword(password, user.Password))
            {
                MessageBox.Show("Авторизация прошла успешно!");
                return true;
            }
            else
            {
                MessageBox.Show("Неправильный пароль!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}