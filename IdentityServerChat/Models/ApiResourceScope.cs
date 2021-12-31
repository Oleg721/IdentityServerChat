using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServerTests.Models
{
    public class ApiResourceScope
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public int ApiResourceId { get; set; }
    }
}
