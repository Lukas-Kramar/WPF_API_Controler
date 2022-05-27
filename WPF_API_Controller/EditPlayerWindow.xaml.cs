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
using WPF_API_Controller.ViewModels;

namespace WPF_API_Controller
{
    /// <summary>
    /// Interaction logic for EditPlayerWindow.xaml
    /// </summary>
    public partial class EditPlayerWindow : Window
    {
        private PlayersViewModel viewmodels;
        public EditPlayerWindow()
        {
            InitializeComponent();
            viewmodels = (PlayersViewModel)DataContext;
            if (viewmodels.SelectedPlayer != null)
            {    
                viewmodels.EditedPlayer = viewmodels.SelectedPlayer;                
            }
        }
        
        private void CloseEditPlayer(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
