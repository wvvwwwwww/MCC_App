using MyCoffeCupApp.data;
using MyCoffeCupApp.Services;
using MyCoffeeCupApp;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyCoffeCupApp
{
    public partial class AutoPage : Page
    {
        private readonly AuthService _authService;

        public AutoPage()
        {
            InitializeComponent();
            _authService = new AuthService();

            // Фокус на поле логина при загрузке
            Loaded += (s, e) => Login.Focus();

            // Обработка нажатия Enter
            Login.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                    Password.Focus();
            };

            Password.KeyDown += async (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                    await PerformLoginAsync();
            };
        }

        private async void btnAut_Click(object sender, RoutedEventArgs e)
        {
            await PerformLoginAsync();
        }

        private async Task PerformLoginAsync()
        {
            // Валидация ввода
            if (string.IsNullOrWhiteSpace(Login.Text))
            {
                ShowError("Введите логин");
                Login.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(Password.Password))
            {
                ShowError("Введите пароль");
                Password.Focus();
                return;
            }

            // Показываем индикатор загрузки
            SetLoadingState(true);

            try
            {
                // 1. Выполняем авторизацию через API
                var (success, user, message) = await _authService.LoginAsync(
                    Login.Text.Trim(),
                    Password.Password
                );

                if (success && user != null)
                {
                    // 2. Сохраняем информацию о пользователе
                    AppState.CurrentUser = new CurrentUser
                    {
                        EmployeeId = user.EmployeeId,
                        EmployeeName = user.EmployeeName,
                        RoleName = user.RoleName,
                        RoleId = user.RoleId
                    };

                    // 3. Получаем дополнительную информацию о сотруднике
                    var employeeInfo = await _authService.GetEmployeeInfoAsync(user.EmployeeId);

                    if (employeeInfo != null)
                    {
                        AppState.CurrentUser.EmployeeName = employeeInfo.Name;
                    }

                    // 4. Логируем успешный вход
                    LogLoginAttempt(true, Login.Text);

                    // 5. Переходим на главную страницу в зависимости от роли
                    NavigateBasedOnRole(user.RoleId);
                }
                else
                {
                    // Логируем неудачную попытку
                    LogLoginAttempt(false, Login.Text);

                    ShowError(message);
                    Password.Clear();
                    Password.Focus();
                }
            }
            catch (Exception ex)
            {
                LogError("Ошибка при авторизации", ex);
                ShowError($"Ошибка соединения с сервером: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void NavigateBasedOnRole(int roleId)
        {
            // Здесь можно настроить навигацию в зависимости от роли
            // Например:
            switch (roleId)
            {
                case 1:
                    AppFrame.frame.Navigate(new SchedulePage());
                    AppFrame.frameMenu.Navigate(new MenuPage());
                    break;;// Администратор
                case 2: // Менеджер (как в вашем примере)
                    AppFrame.frame.Navigate(new SchedulePage());
                    AppFrame.frameMenu.Navigate(new MenuPage());
                    break;
                case 3: // Бариста
                    AppFrame.frame.Navigate(new SchedulePage());
                    break;
                default:
                    AppFrame.frame.Navigate(new SchedulePage());
                    break;
            }

            // Очищаем поля после успешного входа
            Login.Clear();
            Password.Clear();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Обратитесь к администратору для смены пароля или регистрации",
                "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        #region Вспомогательные методы

        private void SetLoadingState(bool isLoading)
        {
            btnAut.IsEnabled = !isLoading;
            btnInfo.IsEnabled = !isLoading;
            Login.IsEnabled = !isLoading;
            Password.IsEnabled = !isLoading;

            if (isLoading)
            {
                btnAut.Content = "Подключение...";
            }
            else
            {
                btnAut.Content = "Войти";
            }

            // Можно добавить ProgressBar или другой индикатор
        }

        private void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "Ошибка авторизации",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        private void LogLoginAttempt(bool success, string login)
        {
            // Здесь можно добавить логирование в файл или БД
            string logMessage = $"{DateTime.Now}: Попытка входа пользователя '{login}' - {(success ? "УСПЕХ" : "НЕУДАЧА")}";

            // Для отладки выводим в Output
            System.Diagnostics.Debug.WriteLine(logMessage);
        }

        private void LogError(string message, Exception ex)
        {
            string errorMessage = $"{DateTime.Now}: {message} - {ex.GetType().Name}: {ex.Message}";
            System.Diagnostics.Debug.WriteLine(errorMessage);
        }

        #endregion
    }
}