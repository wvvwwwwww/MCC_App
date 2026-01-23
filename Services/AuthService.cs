using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace MyCoffeCupApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7124"),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        // Модель для запроса авторизации
        public class LoginRequest
        {
            public string Login { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // Модель ответа от API
        public class LoginResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public UserInfo? User { get; set; }
        }

        // Информация о пользователе
        public class UserInfo
        {
            public int EmployeeId { get; set; }
            public string? EmployeeName { get; set; }
            public string? RoleName { get; set; }
            public int RoleId { get; set; }
        }

        // Метод авторизации через API
        public async Task<(bool Success, UserInfo? User, string Message)> LoginAsync(string login, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    Login = login,
                    Password = password
                };

                // Отправляем POST запрос на эндпоинт авторизации
                var response = await _httpClient.PostAsJsonAsync("api/Autorizations/validate", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    if (result?.Success == true && result.User != null)
                    {
                        return (true, result.User, "Авторизация успешна");
                    }
                    else
                    {
                        return (false, null, result?.Message ?? "Неверный логин или пароль");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return (false, null, $"Ошибка сервера: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Ошибка подключения к серверу: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Ошибка: {ex.Message}");
            }
        }

        // Метод для получения информации о сотруднике по ID
        public async Task<EmployeeInfo?> GetEmployeeInfoAsync(int employeeId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<EmployeeInfo>($"api/Employees/{employeeId}");
            }
            catch
            {
                return null;
            }
        }

        public class EmployeeInfo
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? TitleName { get; set; }
        }
    }
}