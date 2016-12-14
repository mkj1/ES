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
    public class CategoryController : Controller
    {
        private IGeneriskRepository<Category> _categoryRepo;
        private IGeneriskRepository<ComponentType> _typeRepo;
        public CategoryController(IGeneriskRepository<Category> catRepo, IGeneriskRepository<ComponentType> typeRepo)
        {
            _categoryRepo = catRepo;
            _typeRepo = typeRepo;
        }
        private List<int> list;

        public IActionResult Index()
        {
            ////var cat = new Category();
            ////cat.Name = "example";
            ////cat.CategoryId = 1;
            //needs list of all categories

            ViewBag.list = _categoryRepo.GetAll().ToList<Category>();
            return View();
        }

        [Authorize]
        public IActionResult New()
        { 
            var com = new ComponentType();
            com.ComponentName = "hej";
            com.ComponentTypeId = 1;
            //requires list of all component types
            ViewBag.list = _typeRepo.GetAll().ToList<ComponentType>();
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            //needs to create a category and create the binding to the chosen componenttypes
            var cat = new Category();
            cat.Name = model.Name;

            using (var db = new ApplicationDbContext())
            {
                db.Categories.Add(cat);


                if(model.ComponentTypeIds!=null)
                foreach (int id in model.ComponentTypeIds)
                {
                    var type = db.ComponentTypes.Find(id);
                    var cattyp = new CategoryComponentType() { Category = cat, ComponentType = type };
                    db.CategoryComponentTypes.Add(cattyp);
                }
                

                db.SaveChanges();
            }


            return RedirectToAction("", "category", new { area = "" });
        }

        [HttpGet("{category}/[action]/{id}")]
        public IActionResult Show(int id)
        {
            //needs to find a category with its componenttype names in the list
            
            //cat.Name = "hejhejhej";
            //ComponentType com = new ComponentType();
            //com.ComponentName = "efdsdf";
            //com.ComponentTypeId = 1;

            List<ComponentType> tempList = new List<ComponentType>();
            Category cat = new Category();
            using (var db = new ApplicationDbContext())
            {
           
                tempList = db.CategoryComponentTypes.Include(ct => ct.ComponentType)
                             .Where(c => c.CategoryId == id)
                             .Select(c => c.ComponentType).AsNoTracking()
                             .ToList();

                cat = db.Categories.Find(id);

            }



            ViewBag.componentList = tempList;
            ViewBag.category = cat;
            return View();
        }

        [Authorize]
        [HttpGet("{category}/[action]/{id}")]
        public IActionResult Edit(int id)
        {
            //needs to find a category with its componenttypes
            Category cat = new Category();
            cat.Name = "hejhejhej";
            cat.CategoryId = id;
            ComponentType com = new ComponentType();
            com.ComponentName = "efdsdf";
            com.ComponentTypeId = 1;
            ViewBag.componentList = _typeRepo.GetAll().ToList<ComponentType>();
            ViewBag.existingComponentList = _typeRepo.GetAll().ToList<ComponentType>();
            ViewBag.category = cat;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            var edit = new Category { CategoryId = model.Id, Name = model.Name };

            using (var db = new ApplicationDbContext())
            {
                
                db.Categories.Attach(edit);
                var entry = db.Entry(edit);
                entry.Property(e => e.Name).IsModified = true;

                db.CategoryComponentTypes.RemoveRange(db.CategoryComponentTypes.Where(c => c.CategoryId == model.Id));
                db.SaveChanges();

                if (model.ComponentTypeIds != null)
                {
                    foreach (var id in model.ComponentTypeIds)
                    {

                        var type = db.ComponentTypes.Find(id);
                        var cat = db.Categories.Find(model.Id);

                        var cattyp = new CategoryComponentType() { Category = cat, ComponentType = type };
                        db.CategoryComponentTypes.Add(cattyp);
                    }
                }

                db.SaveChanges();
               
            }

            //find category and update with new name and component types
            return RedirectToAction("", "category", new { area = "" });
        }

        [Authorize]
        [HttpGet("{category}/[action]/{id}")]
        public IActionResult Delete(int id)
        {

            using (var db = new ApplicationDbContext())
            {
                var del = new Category { CategoryId = id};
                db.Categories.Remove(del);
                db.SaveChanges();
            }

            return RedirectToAction("", "category", new { area = "" });
        }

    }
}
