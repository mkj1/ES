using EmbeddedStock2.Models;

using System.Collections.Generic;
namespace EmbeddedStock2.ViewModels
{
  public class TypeViewModel{
    public int ComponentTypeId { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }
    public string Location { get; set; }
    public string ImageUrl { get; set; }
    public string Manufacturer { get; set; }

    public List<int> CategoryIds {get; set;}

    public List<int> ComponentIds {get; set;}

  }
}