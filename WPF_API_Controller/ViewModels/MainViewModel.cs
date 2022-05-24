using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_API_Controller.Models;

namespace WPF_API_Controller.ViewModels
{
    internal class MainViewModel : BaseViewModel, INotifyPropertyChanged 
    {

        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value;
            NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public MainViewModel()
        {

            UpdateViewCommand = new ParametrizedRelayCommand<string>(
               (value) =>
               {
                   if(value.ToString() == "Players")
                   {
                       SelectedViewModel = new PlayersViewModel();
                   }
                   if(value.ToString() == "Teams")
                   {
                       SelectedViewModel = new TeamsViewModel();
                   }
               },
               (parameter) => { return true; }
               );
        }
        public ParametrizedRelayCommand<string> UpdateViewCommand { get; set; }
    }
}
