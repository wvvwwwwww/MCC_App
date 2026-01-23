using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MyCoffeCupApp.data;
using MyCoffeCupApp.Employee;
using MyCoffeeCupApp.DTOs;
using MyCoffeeCupApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static MyCoffeeCupApp.DTOs.EmployeeDtos;
using static MyCoffeeCupApp.DTOs.ScheduleDtos;

namespace MyCoffeCupApp
{
    public partial class InfoPage : Page
    {
        private readonly ApiClient _apiClient;
        private EmployeeReadDto? _currentEmployee;
        private List<EmployeeScheduleReadDto> _schedules = new();
        

        public InfoPage()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            Loaded += async (s, e) => await LoadDataAsync();

        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Получаем ID из AppState
                if (AppState.CurrentUser == null)
                {
                    ShowError("Пользователь не авторизован");
                    return;
                }

                int employeeId = AppState.CurrentUser.EmployeeId;

                // Загружаем данные сотрудника
                _currentEmployee = await _apiClient.GetEmployeeByIdAsync(employeeId);
                if (_currentEmployee == null)
                {
                    ShowError("Сотрудник не найден");
                    return;
                }

                // Обновляем UI
                UpdateEmployeeInfo();

                // Загружаем расписание
                await LoadScheduleAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки: {ex.Message}");
            }
        }


        private void UpdateEmployeeInfo()
        {
            if (_currentEmployee == null) return;

            // Ваш существующий XAML код работает с этими TextBlock
            // FIO.Text, Titletx.Text, Num.Text должны быть в XAML
        }

        private async Task LoadScheduleAsync()
        {
            try
            {
                if (_currentEmployee == null) return;

                // Загружаем расписание сотрудника
                var allSchedules = await _apiClient.GetEmployeeSchedulesAsync();
                _schedules = allSchedules
                    .Where(s => s.EmployeeId == _currentEmployee.Id)
                    .OrderBy(s => s.Date)
                    .ToList();

                // Конвертируем для отображения
                var scheduleViews = ConvertToScheduleView(_schedules);
                dbShed.ItemsSource = scheduleViews;

                btnSaveChanges.IsEnabled = _schedules.Any();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки графика: {ex.Message}");
            }
        }

        private List<ScheduleView> ConvertToScheduleView(List<EmployeeScheduleReadDto> schedules)
        {
            var result = new List<ScheduleView>();
            string[] dayNames = { "Воскресенье", "Понедельник", "Вторник", "Среда",
                                  "Четверг", "Пятница", "Суббота" };

            foreach (var schedule in schedules)
            {
                var date = schedule.Date.ToDateTime(TimeOnly.MinValue);
                int dayOfWeek = (int)date.DayOfWeek;

                result.Add(new ScheduleView
                {
                    Id = schedule.Id,
                    DayName = dayNames[dayOfWeek],
                    Date = schedule.Date.ToString("dd.MM.yyyy"),
                    TimeStart = schedule.TimeOfStart.ToString(@"hh\:mm"),
                    TimeEnd = schedule.TimeOfEnd.ToString(@"hh\:mm"),
                    Note = schedule.Note ?? ""
                });
            }

            return result;
        }

        private async void BtnCreateSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null) return;

            var dialog = new EditScheduleWindow();
            if (dialog.ShowDialog() == true && dialog.CreateDto != null)
            {
                try
                {
                    var created = await _apiClient.CreateEmployeeScheduleAsync(dialog.CreateDto);
                    if (created != null)
                    {
                        await LoadScheduleAsync();
                        ShowSuccess("Расписание добавлено!");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Ошибка добавления: {ex.Message}");
                }
            }
        }

        private async void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadScheduleAsync();
                ShowSuccess("Данные обновлены!");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка сохранения: {ex.Message}");
            }
        }

        private async void ButtonH_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null) return;

            try
            {
                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                // Получаем данные через API
                var schedules = await _apiClient.GetSchedulesAsync();
                var employeeSchedules = schedules
                    .Where(s => s.EmployeeId == _currentEmployee.Id)
                    .ToList();

                byte[] fileBytes = GenerateReport(employeeSchedules, startDate, endDate);

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"HoursReport_{DateTime.Now:yyyyMMdd}.xlsx"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    await File.WriteAllBytesAsync(saveDialog.FileName, fileBytes);
                    ShowSuccess("Отчет успешно сохранен!");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при формировании отчета: {ex.Message}");
            }
        }

        private byte[] GenerateReport(List<ScheduleReadDto> schedules, DateTime startDate, DateTime endDate)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Отчет по часам");

                // Заполняем отчет (адаптируйте под ваш формат)
                int row = 1;
                worksheet.Cell(row, 1).Value = $"Отчет по часам: {_currentEmployee?.Name}";
                worksheet.Range(row, 1, row, 5).Merge();
                row += 2;

                // ... остальной код генерации Excel

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // Вспомогательный класс (должен быть у вас)
    public class ScheduleView
    {
        public int Id { get; set; }
        public string DayName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string TimeStart { get; set; } = string.Empty;
        public string TimeEnd { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}