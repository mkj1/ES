using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmbeddedStock2.Models;

namespace EmbeddedStock2.Models
{
    public class Category
    {
        public int CategoryId { get; set;}
        public string Name { get; set; }
        public List<CategoryComponentType> CategoryComponentTypes { get; set; }
    }
}
