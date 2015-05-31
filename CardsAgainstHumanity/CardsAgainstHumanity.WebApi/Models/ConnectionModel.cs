using System;
using System.Collections.Generic;

namespace CardsAgainstHumanity.WebApi.Models
{
    public class Connection
    {
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
    }
}