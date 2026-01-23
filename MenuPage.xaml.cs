using MyCoffeeCupApp;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyCoffeCupApp
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frame.Navigate(new InfoPage());
        }

        private void btnShedule_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frame.Navigate(new SchedulePage());
        }

        private void btnHours_Click(object sender, RoutedEventArgs e)
        {
           // AppFrame.frame.Navigate(new PointsPage());
        }

        private void btnMeetings_Click(object sender, RoutedEventArgs e)
        {
            //AppFrame.frame.Navigate(new MeetingsPage());
        }

        
    }
}
