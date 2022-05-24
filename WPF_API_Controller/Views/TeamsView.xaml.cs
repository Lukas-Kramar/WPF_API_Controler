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
    /// Interaction logic for TeamsView.xaml
    /// </summary>
    public partial class TeamsView : UserControl
    {
        private TeamsViewModel viewmodels;
        public TeamsView()
        {
            InitializeComponent();
            viewmodels = (TeamsViewModel)DataContext;

        }
        private void EditTeam(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                if(viewmodels.SelectedTeam != null)
                {
                    EditTeamWindow editTeamWindow = new EditTeamWindow();
                    editTeamWindow.DataContext = viewmodels;
                    viewmodels.EditedTeam = viewmodels.SelectedTeam;
                    editTeamWindow.Show();
                }
            }
        }
    }
}
