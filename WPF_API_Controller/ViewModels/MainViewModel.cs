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
        private Uri ApiUri = new Uri("http://localhost:3002/");
        private HttpClient _client;

        private string _response;
        private IEnumerable<Player> _resObjPlayers;
        private IEnumerable<Team> _resObjTeams;
        private ObservableCollection<Player> _players = new ObservableCollection<Player>();

        private int _selectedPlayerIndex;
        private Player _selectedPlayer;
        public string _playerParameters;
        private string _teamParameters;
        private int _playerStartedPlaying;
        private string _playerTeamName;

        private ObservableCollection<Team> _teams = new ObservableCollection<Team>();

        public MainViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);
            _playerParameters = "api/Players?";
            ReloadCommand = new RelayCommand(
                async () =>
                {
                    _playerParameters = "api/Players?";
                    if (_playerStartedPlaying != 0) _playerParameters = $"{_playerParameters}startedPlaying={_playerStartedPlaying}&";
                    if (_playerTeamName != null) _playerParameters = $"{_playerParameters}teamName={_playerTeamName}"; Console.WriteLine("Tried Team Name");
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync(_playerParameters);
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObjPlayers = JsonConvert.DeserializeObject<IEnumerable<Player>>(Response);
                        Players = new ObservableCollection<Player>(_resObjPlayers);
                    }
                    else
                    {
                        Response = "Players Ooopss";
                        Players.Clear();
                    }

                    response = await _client.GetAsync("api/Teams");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObjTeams = JsonConvert.DeserializeObject<IEnumerable<Team>>(Response);
                        Teams = new ObservableCollection<Team>(_resObjTeams);
                    }
                    else
                    {
                        Response = "Players Ooopss";
                        Players.Clear();
                    }
                }
                );
        }

        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }

        public int SelectedPlayerIndex { get { return _selectedPlayerIndex + 1; } set { _selectedPlayerIndex = value; NotifyPropertyChanged(); /*Remove.RaiseCanExecuteChanged();*/ } }
        public Player SelectedPlayer { get { return _selectedPlayer; } set { _selectedPlayer = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Player> Players { get { return _players; } set { _players = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Team> Teams { get { return _teams; } set { _teams = value; NotifyPropertyChanged(); } }
        public int StartedPlaying { get { return _playerStartedPlaying; } set { _playerStartedPlaying = value; NotifyPropertyChanged(); } }
        public string TeamName { get { return _playerTeamName; } set { _playerTeamName = value; NotifyPropertyChanged(); } }

        public RelayCommand ReloadCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
