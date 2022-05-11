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
using WPF_API_Controller.Models;

namespace WPF_API_Controller.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Uri ApiUri = new Uri("http://localhost:3000/");
        private HttpClient _client;

        private string _response;
        private IEnumerable<Player> _resObj;
        private ObservableCollection<Player> _players = new ObservableCollection<Player>();

        private int _selectedPlayerIndex;
        private Player _selectedPlayer;

        public MainViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);
            ReloadCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("api/Players");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObj = JsonConvert.DeserializeObject<IEnumerable<Player>>(Response);
                        Players = new ObservableCollection<Player>(_resObj);
                    }
                    else
                    {
                        Response = "OOPS";
                        Players.Clear();
                    }
                }
                );
        }

        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }

        public int SelectedPlayerIndex { get { return _selectedPlayerIndex + 1; } set { _selectedPlayerIndex = value; NotifyPropertyChanged(); /*Remove.RaiseCanExecuteChanged();*/ } }
        public Player SelectedPlayer { get { return _selectedPlayer; } set { _selectedPlayer = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Player> Players { get { return _players; } set { _players = value; NotifyPropertyChanged(); } }

        public RelayCommand ReloadCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
