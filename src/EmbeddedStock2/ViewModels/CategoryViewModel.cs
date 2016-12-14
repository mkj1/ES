using EmbeddedStock2.Models;

using System.Collections.Generic;
namespace EmbeddedStock2.ViewModels
{
  public class CategoryViewModel{
    public int Id{get; set;}
    public string Name { get; set; }
    public List<int> ComponentTypeIds{ get; set; }

    public List<ComponentType> ComponentTypes {get; set;}

  }
}