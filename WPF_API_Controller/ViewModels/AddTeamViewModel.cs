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
    internal class AddTeamViewModel : BaseViewModel
    {
        private Uri ApiUri = new Uri("http://localhost:3002/");
        private HttpClient _client;
        private string _playersURL = "api/Players?";
        private string _addTeamURL = "api/Teams";

        private NewTeam _newTeam = new NewTeam();

        private IEnumerable<Player> _resObjPlayers;
        private string _resNewTeam;
        private string _response;

        private ObservableCollection<Player> _players = new ObservableCollection<Player>();

        public AddTeamViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);

            //TeamsGET = new RelayCommand(
            //        async () =>
            //        {
            //            HttpResponseMessage response = new HttpResponseMessage();
            //            response = await _client.GetAsync(_playersURL);
            //            if (response.IsSuccessStatusCode)
            //            {
            //                Response = await response.Content.ReadAsStringAsync();
            //                _resObjPlayers = JsonConvert.DeserializeObject<IEnumerable<Player>>(Response);
            //                Players = new ObservableCollection<Player>(_resObjPlayers);
            //            }
            //            else
            //            {
            //                Response = "Teams OOOPS";
            //                Players.Clear();
            //            }
            //        }
            //    );
            //TeamsGET.Execute("");

            //NewTeam.Players = new List<Player>();

            TeamPOST = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    _resNewTeam = JsonConvert.SerializeObject(NewTeam);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(_resNewTeam);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await _client.PostAsync(_addTeamURL, byteContent);
                });
        }
        public NewTeam NewTeam
        {
            get { return _newTeam; }
            set { _newTeam = value; }
        }
        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }
        public ObservableCollection<Player> Players
        {
            get { return _players; }
            set { _players = value; NotifyPropertyChanged(); }
        }

        public RelayCommand TeamsGET { get; set; }
        public RelayCommand TeamPOST { get; set; }
    }
}
