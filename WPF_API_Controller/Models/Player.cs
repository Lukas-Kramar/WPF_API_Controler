using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WPF_API_Controller.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public int? TeamId { get; set; }
        [JsonIgnore]
        public Team? Team { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public int StartedPlaying { get; set; }
        public int MoneyWon { get; set; }
        public int NumberOfTournamentsPlayed { get; set; }
        public int BiggestPrizeWon { get; set; }
    }
}
