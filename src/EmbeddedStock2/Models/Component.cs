using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmbeddedStock2.Models
{
    public class Component
    {
        public int ComponentId { get; set; }
        public int ComponentTypeId { get; set; }
        public int ComponentNumber { get; set; }
        public string SerialNo { get; set; }
        public string SearchTerm { get; set; }
    }
}
