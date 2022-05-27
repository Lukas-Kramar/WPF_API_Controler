using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WPF_API_Controller.Models;

namespace WPF_API_Controller.ViewModels
{
    internal class PlayersViewModel : BaseViewModel
    {
        private Uri ApiUri = new Uri("http://localhost:3002/");
        private HttpClient _client;

        private int _selectedPlayerIndex;
        private Player _selectedPlayer;
        public string _playerParameters;
        private int _playerStartedPlaying;
        private string _playerTeamName;

        private IEnumerable<Team> _resObjTeams;

        private ObservableCollection<Team> _teams = new ObservableCollection<Team>();
        private ObservableCollection<Player> _players = new ObservableCollection<Player>();

        private string _response;
        private IEnumerable<Player> _resObjPlayers;

        private Player _editedPlayer;
        private string _editPlayerURL = "api/Players/";
        private string _resEditedPlayer;

        public PlayersViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);
            _playerParameters = "api/Players?";


            PlayersGET = new RelayCommand(
                    async () =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();

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

                        _playerParameters = "api/Players?";
                        if (_playerStartedPlaying != 0) _playerParameters = $"{_playerParameters}startedPlaying={_playerStartedPlaying}&";
                        if (_playerTeamName != null) _playerParameters = $"{_playerParameters}teamName={_playerTeamName}"; Console.WriteLine("Tried Team Name");
                       
                        response = await _client.GetAsync(_playerParameters);
                        if (response.IsSuccessStatusCode)
                        {
                            Response = await response.Content.ReadAsStringAsync();
                            _resObjPlayers = JsonConvert.DeserializeObject<IEnumerable<Player>>(Response);
                            Players = new ObservableCollection<Player>(_resObjPlayers);
                            foreach (var player in Players)
                            {
                                if (player.TeamId > 0) player.Team = Teams.Where(x => x.TeamId == player.TeamId).FirstOrDefault();
                            }
                        }
                        else
                        {
                            Response = "Players Ooopss";
                            Players.Clear();
                        }

                        
                    }
                );

            PlayerPUT = new RelayCommand(
                async () =>
                {
                    if(EditedPlayer.Team.TeamId != null)
                    {
                        var idTeam = EditedPlayer.Team.TeamId;
                        EditedPlayer.TeamId = idTeam;
                    }                    
                    _editPlayerURL = $"{_editPlayerURL}{EditedPlayer.PlayerId}";
                    HttpResponseMessage response = new HttpResponseMessage();
                    _resEditedPlayer = JsonConvert.SerializeObject(EditedPlayer);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(_resEditedPlayer);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await _client.PutAsync(_editPlayerURL, byteContent);
                });

            PlayerDELETE = new RelayCommand(
                    async () =>
                    {
                        _playerParameters = "api/Players/";
                        if (SelectedPlayer.PlayerId >0 ) _playerParameters = _playerParameters + SelectedPlayer.PlayerId.ToString();
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = await _client.DeleteAsync(_playerParameters);
                        if (response.IsSuccessStatusCode)
                        {
                            Response = await response.Content.ReadAsStringAsync();
                            //_resObjPlayers = JsonConvert.DeserializeObject<IEnumerable<Player>>(Response);
                            //Players = new ObservableCollection<Player>(_resObjPlayers);
                        }
                        else
                        {
                            Response = "Player Doesnt Exist! Error: 404";                            
                        }
                        PlayersGET.Execute("");
                    }
                );

            ReloadCommand = new RelayCommand(
                () =>
                {
                    PlayersGET.Execute("");
                },
                () => { return true; }
                );
            PlayersGET.Execute("");
            //Teams.ToList();
        }

        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }

        public int SelectedPlayerIndex { get { return _selectedPlayerIndex + 1; } set { _selectedPlayerIndex = value; NotifyPropertyChanged(); /*Remove.RaiseCanExecuteChanged();*/ } }
        public Player SelectedPlayer { get { return _selectedPlayer; } set { _selectedPlayer = value; NotifyPropertyChanged(); } }
        public Player EditedPlayer { get { return _editedPlayer; } set { _editedPlayer = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Player> Players { get { return _players; } set { _players = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Team> Teams { get { return _teams; } set { _teams = value; NotifyPropertyChanged(); } }
        public int StartedPlaying { get { return _playerStartedPlaying; } set { _playerStartedPlaying = value; NotifyPropertyChanged(); } }
        public string TeamName { get { return _playerTeamName; } set { _playerTeamName = value; NotifyPropertyChanged(); } }        

        public RelayCommand PlayersGET { get; set; }
        public RelayCommand PlayerDELETE { get; set; }
        public RelayCommand PlayerPUT { get; set; }
        public RelayCommand ReloadCommand { get; set; }
    }
}
