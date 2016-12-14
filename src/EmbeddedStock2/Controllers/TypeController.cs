using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmbeddedStock2.Models;
using EmbeddedStock2.Repositories;
using EmbeddedStock2.ViewModels;
using Microsoft.EntityFrameworkCore;
using EmbeddedStock2.Data;
using Microsoft.AspNetCore.Authorization;

namespace EmbeddedStock2.Controllers
{
    public class TypeController : Controller
    {
        private IGeneriskRepository<Category> _categoryRepo;
        private IGeneriskRepository<ComponentType> _typeRepo;
        public TypeController(IGeneriskRepository<Category> catRepo, IGeneriskRepository<ComponentType> typeRepo)
        {
            _categoryRepo = catRepo;
            _typeRepo = typeRepo;
        }
        private List<int> list;

        public IActionResult Index()
        {
            
            //needs list of all types
            ViewBag.list = _typeRepo.GetAll().ToList<ComponentType>();
            return View();
        }

        [Authorize]
        public IActionResult New()
        { 
            var cat = new Category();
            cat.Name = "hej";
            cat.CategoryId = 1;
            //requires list of all categories
            ViewBag.categorylist = _categoryRepo.GetAll().ToList<Category>();
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(TypeViewModel model)
        {
            var com = new ComponentType();
            com.ComponentName = model.Name;
            com.ComponentInfo = model.Info;
            com.ImageUrl = model.ImageUrl;
            com.Location = model.Location;
            com.Manufacturer = model.Manufacturer;

            _typeRepo.Insert(com);

            //create new type and associate with chosen categories
     
            return RedirectToAction("", "type", new { area = "" });
        }


        [HttpGet("[controller]/[action]/{id}")]
        public IActionResult Show(int id)
        {
            
            Category cat = new Category();
            cat.Name = "hejhejhej";
            //ComponentType comtype = new ComponentType();
            //comtype.ComponentName = "efdsdf";
            //comtype.ComponentTypeId = 1;
            //var com = new Component();
            //com.ComponentId = 1;
            //com.ComponentNumber = 3;
            //needs to find a Componenttype with its categories names and components in 2 lists

            List<Component> tempList = new List<Component>();
            ComponentType tempComp = new ComponentType();
            List<Category> tempCatlist = new List<Category>();

            using (var db = new ApplicationDbContext())
            {


                var query = from c in db.Components
                           where c.ComponentTypeId == id
                           select c;

                tempList = query.ToList();

                tempComp = db.ComponentTypes.Find(id);


                tempCatlist = db.CategoryComponentTypes.Include(ct => ct.Category)
                             .Where(c => c.ComponentTypeId == id)
                             .Select(c => c.Category)
                             .ToList();

            }

            ViewBag.componentList = tempList;



            var stringlist = new List<string> ();
            foreach ( Category cate in tempCatlist)
            {
                stringlist.Add(cate.Name);
            }

            ViewBag.categoryNames = stringlist;
            ViewBag.component_type = tempComp;
            return View();
        }
    }
}
