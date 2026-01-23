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

namespace MyCoffeCupApp
{
    /// <summary>
    /// Логика взаимодействия для ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        private DateTime currentMonth;
        public ScheduleWindow()
        {
            InitializeComponent();
            currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            LoadCalendar();
        }

        private void LoadCalendar()
        {
            try
            {
                // Полностью очищаем контейнер
                calendarContainer.Children.Clear();
                calendarContainer.RowDefinitions.Clear();
                calendarContainer.ColumnDefinitions.Clear();

                // Устанавливаем название месяца
                txtMonthYear.Text = currentMonth.ToString("MMMM yyyy").ToUpper();

                // Создаем 7 колонок (дни недели)
                for (int i = 0; i < 7; i++)
                {
                    calendarContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }

                // Создаем 6 строк (максимальное количество недель в месяце)
                for (int i = 0; i < 6; i++)
                {
                    calendarContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }

                // Получаем первый день месяца
                DateTime firstDay = currentMonth;
                int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);

                // Определяем позицию первого дня (понедельник = 0, воскресенье = 6)
                int startColumn = (int)firstDay.DayOfWeek - 1;
                if (startColumn < 0) startColumn = 6; // Воскресенье

                int currentRow = 0;
                int currentColumn = startColumn;

                // Добавляем дни месяца
                for (int day = 1; day <= daysInMonth; day++)
                {
                    DateTime currentDate = new DateTime(currentMonth.Year, currentMonth.Month, day);
                    DayOfWeek dayOfWeek = currentDate.DayOfWeek;

                    // Создаем кнопку дня
                    Button dayButton = new Button
                    {
                        Content = day.ToString(),
                        Margin = new Thickness(2),
                        Padding = new Thickness(5),
                        Tag = DateOnly.FromDateTime(currentDate),
                        FontSize = 14,
                        Height = 40,
                        Background = Brushes.White,
                        BorderBrush = Brushes.LightGray
                    };



                    // Выделяем сегодняшний день
                    if (currentDate.Date == DateTime.Today)
                    {
                        dayButton.Background = Brushes.LightGray;
                        dayButton.FontWeight = FontWeights.Bold;
                        dayButton.BorderBrush = Brushes.Orange;
                        dayButton.BorderThickness = new Thickness(2);
                    }

                    // Назначаем обработчик
                    dayButton.Click += DayButton_Click;

                    // Устанавливаем позицию в Grid
                    Grid.SetRow(dayButton, currentRow);
                    Grid.SetColumn(dayButton, currentColumn);

                    // Добавляем в контейнер
                    calendarContainer.Children.Add(dayButton);

                    // Переходим к следующей ячейке
                    currentColumn++;
                    if (currentColumn > 6)
                    {
                        currentColumn = 0;
                        currentRow++;
                    }
                }

                // Удаляем пустые строки внизу
                bool lastRowEmpty = true;
                for (int col = 0; col < 7; col++)
                {
                    foreach (UIElement child in calendarContainer.Children)
                    {
                        if (Grid.GetRow(child) == currentRow && Grid.GetColumn(child) == col)
                        {
                            lastRowEmpty = false;
                            break;
                        }
                    }
                    if (!lastRowEmpty) break;
                }

                if (lastRowEmpty && currentRow > 0)
                {
                    calendarContainer.RowDefinitions[currentRow].Height = new GridLength(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания календаря: {ex.Message}");
            }
        }


        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is DateOnly selectedDate)
                {
                    // Преобразуем DateOnly в DateTime для окна
                    DateTime dateTime = selectedDate.ToDateTime(TimeOnly.MinValue);

                    // Открываем окно
                    var dayScheduleWindow = new DayScheduleWindow(dateTime)
                    {
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };

                    dayScheduleWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия окна: {ex.Message}");
            }
        }

        private void btnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = currentMonth.AddMonths(1);
            LoadCalendar();
        }

        private void btnPrevMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = currentMonth.AddMonths(-1);
            LoadCalendar();
        }

        

    }
}
