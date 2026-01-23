using MyCoffeCupApp.data;
using MyCoffeeCupApp.DTOs;
using MyCoffeeCupApp.Services;
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

namespace MyCoffeCupApp.Employee
{
    /// <summary>
    /// Логика взаимодействия для EditScheduleWindow.xaml
    /// </summary>
    public partial class EditScheduleWindow : Window
    {
        private readonly ApiClient _apiClient;
        private EmployeeScheduleReadDto? _existingSchedule;

        public EmployeeScheduleCreateDto? CreateDto { get; private set; }
        public EmployeeScheduleUpdateDto? UpdateDto { get; private set; }
        public int? ScheduleId => _existingSchedule?.Id;
        public EditScheduleWindow()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            dpDate.SelectedDate = DateTime.Today;
        }
        public EditScheduleWindow(EmployeeScheduleReadDto schedule)
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            _existingSchedule = schedule;

            dpDate.SelectedDate = schedule.Date.ToDateTime(TimeOnly.MinValue);
            txtStart.Text = schedule.TimeOfStart.ToString(@"hh\:mm");
            txtEnd.Text = schedule.TimeOfEnd.ToString(@"hh\:mm");
            txtNote.Text = schedule.Note ?? "";

            Title = "Редактировать расписание";
            btnAdd.Content = "Сохранить";

        }



        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                var selectedDate = DateOnly.FromDateTime(dpDate.SelectedDate ?? DateTime.Today);

                // ПРАВИЛЬНОЕ преобразование времени
                TimeSpan startTime = TimeSpan.Parse(txtStart.Text);
                TimeSpan endTime = TimeSpan.Parse(txtEnd.Text);

                var note = txtNote.Text.Trim();

                // Получаем ID текущего сотрудника
                int currentEmployeeId = GetCurrentEmployeeId();

                if (_existingSchedule == null)
                {
                    // Создание нового расписания
                    CreateDto = new EmployeeScheduleCreateDto
                    {
                        Date = selectedDate,
                        TimeOfStart = startTime,  // TimeSpan
                        TimeOfEnd = endTime,      // TimeSpan
                        Note = note,
                        EmployeeId = currentEmployeeId
                    };
                }
                else
                {
                    // Обновление существующего
                    UpdateDto = new EmployeeScheduleUpdateDto
                    {
                        Date = selectedDate,
                        TimeOfStart = startTime,  // TimeSpan
                        TimeOfEnd = endTime,      // TimeSpan
                        Note = note,
                        EmployeeId = currentEmployeeId
                    };
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetCurrentEmployeeId()
        {
            if (AppState.CurrentUser == null)
            {
                MessageBox.Show("Пользователь не авторизован", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }

            return AppState.CurrentUser.EmployeeId;
        }

        private bool ValidateInputs()
        {
            // Проверка времени начала
            if (!TimeSpan.TryParse(txtStart.Text, out TimeSpan start))
            {
                MessageBox.Show("Введите корректное время начала в формате ЧЧ:ММ",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtStart.Focus();
                txtStart.SelectAll();
                return false;
            }

            // Проверка времени окончания
            if (!TimeSpan.TryParse(txtEnd.Text, out TimeSpan end))
            {
                MessageBox.Show("Введите корректное время окончания в формате ЧЧ:ММ",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEnd.Focus();
                txtEnd.SelectAll();
                return false;
            }

            // Проверка что окончание позже начала
            if (end <= start)
            {
                MessageBox.Show("Время окончания должно быть позже времени начала",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEnd.Focus();
                txtEnd.SelectAll();
                return false;
            }

            // Проверка даты
            if (!dpDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDate.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}