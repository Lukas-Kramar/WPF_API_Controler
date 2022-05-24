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

namespace WPF_API_Controller.Views
{
    /// <summary>
    /// Interaction logic for PlayersView.xaml
    /// </summary>
    public partial class PlayersView : UserControl
    {
        private PlayersViewModel viewmodels;
        public PlayersView()
        {
            InitializeComponent();
            viewmodels = (PlayersViewModel)DataContext;
        }
        private void EditPlayer(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                if (viewmodels.SelectedPlayer != null)
                {
                    EditPlayerWindow editPlayerWindow = new EditPlayerWindow();
                    editPlayerWindow.DataContext = viewmodels;
                    viewmodels.EditedPlayer = viewmodels.SelectedPlayer;
                    editPlayerWindow.Show();
                }
            }
        }
    }

}
