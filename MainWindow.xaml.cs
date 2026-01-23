using Microsoft.EntityFrameworkCore;
using MyCoffeCupApp.Models;
using MyCoffeeCupApp;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppFrame.frame = frmMain;
            AppFrame.frameMenu = Menu;
            frmMain.Navigate(new AutoPage());
        }

    }
}