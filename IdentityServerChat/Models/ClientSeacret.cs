using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServerTests.Models
{
    public class ClientSecrets
    {
        public int Id { get; set; }
        public string Secrets { get; set; }
        public int ClientId { get; set; }
    }
}
