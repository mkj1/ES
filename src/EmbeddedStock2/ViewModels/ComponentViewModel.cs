using EmbeddedStock2.Models;

using System.Collections.Generic;
namespace EmbeddedStock2.ViewModels
{
  public class ComponentViewModel{
    public int ComponentId { get; set; }
    //public ComponentType ComponentType { get; set; }
    public int ComponentTypeId { get; set; }
    public int ComponentNumber { get; set; }
    public string SerialNo { get; set; }

    public string SearchTerm { get; set;}

  }
}