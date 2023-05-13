using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Попрыженок.Models
{
    public partial class Agent
    {
        public Agent()
        {
            Realization = new HashSet<Realization>();
        }

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string AgentType { get; set; }
        public string Adress { get; set; }
        public string Itn { get; set; }
        public string CoIe { get; set; }
        public string DirectorName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public int Priority { get; set; }

        public virtual ICollection<Realization> Realization { get; set; }
    }
}
