using System.Collections.Generic;
using APBDTest1.Models;

namespace APBDTest1
{
    public class TeamMemberData
    {
        public string IdTeamMember { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public List<Task> Tasks { get; set; }

    }
}