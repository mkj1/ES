using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmbeddedStock2.Models;

namespace EmbeddedStock2.Models
{
    public class ComponentType
    {
        public int ComponentTypeId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentInfo { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public string Manufacturer { get; set; }

        public List<Component> Components { get; protected set; }
        public List<CategoryComponentType> CategoryComponentTypes { get; set; }

    }
}
