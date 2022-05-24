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
using WPF_API_Controller.ViewModels;

namespace WPF_API_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();   
        }

        private void AddPlayer(object sender, RoutedEventArgs e)
        {
            if(sender is MenuItem)
            {
                AddPlayerWindow addPlayerWindow = new AddPlayerWindow();
                addPlayerWindow.Show();         
            }
        }

        private void AddTeam(object sender, RoutedEventArgs e)
        {
            if(sender is MenuItem)
            {
                AddTeamWindow addTeamWindow = new AddTeamWindow();
                addTeamWindow.Show();
            }
        }
    }
}
