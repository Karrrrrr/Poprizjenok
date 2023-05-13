using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Попрыженок.Models
{
    public partial class Realization
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public int AgentId { get; set; }

        public virtual Agent Agent { get; set; }
    }
}
