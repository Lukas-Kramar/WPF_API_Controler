using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_API_Controller.Models
{
    public class NewPlayer
    {
        public int? TeamId { get; set; }
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
