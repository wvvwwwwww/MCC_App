using Microsoft.EntityFrameworkCore;
using MyCoffeeCupApp;
using MyCoffeeCupApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyCoffeCupApp
{
    /// <summary>
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        private readonly ApiClient _apiClient;
        private DateOnly _currentDate;


        public SchedulePage()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            _currentDate = DateOnly.FromDateTime(DateTime.Today);
            Loaded += async (s, e) => await LoadScheduleAsync();
        }

        private async Task LoadScheduleAsync()
        {
            spSchedule.Children.Clear();

            // Ваш XAML должен содержать txtDate и txtDayOfWeek
            txtDate.Text = _currentDate.ToString("dd.MM.yyyy");
            txtDayOfWeek.Text = GetRussianDayOfWeek(_currentDate.DayOfWeek);

            try
            {
                var points = await _apiClient.GetPointsAsync();
                var allSchedules = await _apiClient.GetSchedulesAsync();
                var employees = await _apiClient.GetEmployeesAsync();

                var employeeDict = employees.ToDictionary(e => e.Id);

                var schedules = allSchedules
                    .Where(s => s.Date == _currentDate)
                    .ToList();

                bool hasAnySchedule = false;

                foreach (var point in points.OrderBy(p => p.Id))
                {
                    var pointStack = new System.Windows.Controls.StackPanel
                    {
                        Margin = new Thickness(0, 10, 0, 10)
                    };

                    pointStack.Children.Add(new System.Windows.Controls.TextBlock
                    {
                        Text = point.Address,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 5)
                    });

                    var pointSchedules = schedules
                        .Where(s => s.PointId == point.Id)
                        .ToList();

                    if (!pointSchedules.Any())
                    {
                        pointStack.Children.Add(new System.Windows.Controls.TextBlock
                        {
                            Text = "Нет сотрудников",
                            FontStyle = FontStyles.Italic,
                            Foreground = System.Windows.Media.Brushes.Gray,
                            Margin = new Thickness(20, 0, 0, 0)
                        });
                    }
                    else
                    {
                        hasAnySchedule = true;

                        foreach (var schedule in pointSchedules)
                        {
                            if (employeeDict.TryGetValue(schedule.EmployeeId, out var employee))
                            {
                                pointStack.Children.Add(new System.Windows.Controls.TextBlock
                                {
                                    Text = $"{employee.Name} " +
                                           $"с {schedule.TimeOfStart:hh\\:mm} до {schedule.TimeOfEnd:hh\\:mm}",
                                    Margin = new Thickness(20, 2, 0, 2)
                                });
                            }
                        }
                    }

                    spSchedule.Children.Add(pointStack);
                }

                // Ваш XAML должен содержать txtNoSchedule
                txtNoSchedule.Visibility = hasAnySchedule ? Visibility.Collapsed : Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetRussianDayOfWeek(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Понедельник",
                DayOfWeek.Tuesday => "Вторник",
                DayOfWeek.Wednesday => "Среда",
                DayOfWeek.Thursday => "Четверг",
                DayOfWeek.Friday => "Пятница",
                DayOfWeek.Saturday => "Суббота",
                DayOfWeek.Sunday => "Воскресенье",
                _ => ""
            };
        }

        private async void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddDays(-1);
            await LoadScheduleAsync();
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddDays(1);
            await LoadScheduleAsync();
        }

        private void btnMonth_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}