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

        private void NavigationToMainMenu(Users user)
        {
            new MainMenu(user).Show();
            this.Close();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActionSelectionCheckBox.IsChecked.Value)
            {
                var registrationResult = Registration(LoginTextBox.Text, PasswordTextBox.Password);
                if (registrationResult != null)
                {
                    NavigationToMainMenu(registrationResult);
                }
            }
            else
            {
                var authenticationResult = Authorization(LoginTextBox.Text, PasswordTextBox.Password);

                if (authenticationResult != null)
                {
                    NavigationToMainMenu(authenticationResult);
                }
            }
        }

        public Users Registration(string login, string password)
        {
            if (new[] { login, password }.Any(x => String.IsNullOrWhiteSpace(x)))
            {
                MessageBox.Show("Проверьте заполненность полей!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            if (password.Length < 8 || password.Length > 20)
            {
                MessageBox.Show("Минимальная длина пароля - 8 символов, а максимальная длина - 20 символов", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            if (UserExists(login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            var user = new Users { Login = login, Password = HashingData.HashingPassword.HashPassword(password) };
            RegisterUser(user);
            MessageBox.Show("Вы успешно зарегистрировались", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);

            return user;

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

        public Users Authorization(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Проверьте заполненность полей!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            var user = DBContext.Context.Users.FirstOrDefault(x => x.Login == login);

            if (user == null)
            {
                MessageBox.Show("Пользователь не найден, Вам нужно пройти регистрацию", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            if (HashingPassword.VerifyPassword(password, user.Password))
            {
                MessageBox.Show("Авторизация прошла успешно!");
                return user;
            }
            else
            {
                MessageBox.Show("Неправильный пароль!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
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