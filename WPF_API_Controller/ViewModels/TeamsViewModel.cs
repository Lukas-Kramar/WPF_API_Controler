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
    internal class TeamsViewModel : BaseViewModel
    {
        private Uri ApiUri = new Uri("http://localhost:3002/");
        private HttpClient _client;

        private int _selectedTeamIndex;
        private Team _selectedTeam;
        private string _teamParameters;
        private string _teamCountry;

        private IEnumerable<Team> _resObjTeams;
        private IEnumerable<Player> _resObjPlayers;
        private string _response;

        private ObservableCollection<Team> _teams = new ObservableCollection<Team>();
        private ObservableCollection<Player> _players = new ObservableCollection<Player>();
        private ObservableCollection<Player> _teamsplayers = new ObservableCollection<Player>();

        private Team _editedTeam;
        private string _editTeamURL = "api/Teams/";
        private string _resEditedTeam;

        public TeamsViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);
            _teamParameters = "api/Teams?";


            TeamsGET = new RelayCommand(
                    async () =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = await _client.GetAsync("api/Players");
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

                        _teamParameters = "api/Teams?";
                        if (_teamCountry != null) _teamParameters = $"{_teamParameters}country={_teamCountry}"; Console.WriteLine("Tried Team Name");

                        response = await _client.GetAsync(_teamParameters);
                        if (response.IsSuccessStatusCode)
                        {
                            Response = await response.Content.ReadAsStringAsync();
                            _resObjTeams = JsonConvert.DeserializeObject<IEnumerable<Team>>(Response);
                            Teams = new ObservableCollection<Team>(_resObjTeams);
                            foreach(var team in Teams)
                            {
                                team.Players = Players.Where(player => player.TeamId == team.TeamId).ToList();
                            }
                        }
                        else
                        {
                            Response = "Players Ooopss";
                            Players.Clear();
                        }

                    }
                );
            TeamsGET.Execute("");

            TeamDELETE = new RelayCommand(
                    async () =>
                    {
                        _teamParameters = "api/Teams/";
                        if (SelectedTeam.TeamId > 0) _teamParameters = _teamParameters + SelectedTeam.TeamId.ToString();
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = await _client.DeleteAsync(_teamParameters);
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
                        TeamsGET.Execute("");
                    }
                );
            TeamPUT = new RelayCommand(
                async () =>
                {
                    _editTeamURL = $"{_editTeamURL}{EditedTeam.TeamId}";
                    HttpResponseMessage response = new HttpResponseMessage();
                    _resEditedTeam = JsonConvert.SerializeObject(EditedTeam);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(_resEditedTeam);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await _client.PutAsync(_editTeamURL, byteContent);
                });

            ReloadCommand = new RelayCommand(
                () =>
                {
                    TeamsGET.Execute("");
                },
                () => { return true; }
                );
        }
        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public int SelectedTeamIndex { get { return _selectedTeamIndex + 1; } set { _selectedTeamIndex = value; NotifyPropertyChanged(); /*Remove.RaiseCanExecuteChanged();*/ } }
        public Team SelectedTeam { get { return _selectedTeam; } set { _selectedTeam = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Team> Teams { get { return _teams; } set { _teams = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Player> Players { get { return _players; } set { _players = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Player> TeamsPlayers { get { return _players; } set { _players = value; NotifyPropertyChanged(); } }
        public string TeamCountry { get { return _teamCountry; } set { _teamCountry = value; NotifyPropertyChanged(); } }

        public Team EditedTeam
        {
            get { return _editedTeam; }
            set { _editedTeam = value; }
        }

        public RelayCommand TeamsGET { get; set; }
        public RelayCommand ReloadCommand { get; set; }
        public RelayCommand TeamDELETE { get; set; }
        public RelayCommand TeamPUT { get; set; }
    }
}
