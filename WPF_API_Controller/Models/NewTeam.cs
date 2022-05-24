using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_API_Controller.Models
{
    public class NewTeam
    {
        public ICollection<Player> Players { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Owner { get; set; }
        public int MembersCount { get; set; }
        public int MoneyWon { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
