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
    internal class AddPlayerViewModel : BaseViewModel
    {
        private Uri ApiUri = new Uri("http://localhost:3002/");
        private HttpClient _client;
        private string _teamsURL = "api/Teams?";
        private string _addPlayerURL = "api/Players";

        private NewPlayer _newPlayer = new NewPlayer();

        private IEnumerable<Team> _resObjTeams;
        private string _resNewPlayer;
        private string _response;

        private ObservableCollection<Team> _teams = new ObservableCollection<Team>();

        public AddPlayerViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);

            TeamsGET = new RelayCommand(
                    async () =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = await _client.GetAsync(_teamsURL);
                        if (response.IsSuccessStatusCode)
                        {
                            Response = await response.Content.ReadAsStringAsync();
                            _resObjTeams = JsonConvert.DeserializeObject<IEnumerable<Team>>(Response);
                            Teams = new ObservableCollection<Team>(_resObjTeams);
                        }
                        else
                        {
                            Response = "Teams OOOPS";
                            Teams.Clear();
                        }
                    }
                );
            TeamsGET.Execute("");

            PlayerPOST = new RelayCommand(
                async () =>
                {
                    var idTeam = NewPlayer.Team.TeamId;
                    NewPlayer.TeamId = idTeam;
                    HttpResponseMessage response = new HttpResponseMessage();
                    _resNewPlayer = JsonConvert.SerializeObject(NewPlayer);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(_resNewPlayer);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await _client.PostAsync(_addPlayerURL, byteContent);
                });
        }

        public NewPlayer NewPlayer
        {
            get { return _newPlayer; }
            set { _newPlayer = value; }
        }
        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }
        public ObservableCollection<Team> Teams {
            get { return _teams; }
            set { _teams = value; NotifyPropertyChanged(); }
        }

        public RelayCommand TeamsGET { get; set; }
        public RelayCommand PlayerPOST { get; set; }
    }
}
