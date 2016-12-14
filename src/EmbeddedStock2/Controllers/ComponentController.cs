using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmbeddedStock2.Models;
using EmbeddedStock2.Repositories;
using EmbeddedStock2.ViewModels;
using EmbeddedStock2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EmbeddedStock2.Controllers
{
    public class ComponentController : Controller
    {
        private IGeneriskRepository<Category> _categoryRepo;
        private IGeneriskRepository<ComponentType> _typeRepo;
        public ComponentController(IGeneriskRepository<Category> catRepo, IGeneriskRepository<ComponentType> typeRepo)
        {
            _categoryRepo = catRepo;
            _typeRepo = typeRepo;
        }
        private List<int> list;

        public IActionResult Index()
        {
            var cat = new ComponentViewModel();
            cat.ComponentNumber = 1231345454;
            //needs list of all component
            cat.SerialNo = "12a3ds12a";
            cat.ComponentId = 2;
            var com = new ComponentType();
            com.ComponentName = "hej";
            com.ComponentTypeId = 1;

            using (var db = new ApplicationDbContext())
            {
                ViewBag.list = db.Components.AsNoTracking().ToList<Component>();
            }
             
            return View();
        }

        [Authorize]
        public IActionResult New()
        {
            var cat = new ComponentViewModel();
            cat.ComponentNumber = 1231345454;
            //needs list of all component
            cat.SerialNo = "12a3ds12a";
            cat.ComponentId = 2;
            var com = new ComponentType();
            com.ComponentName = "hej";
            com.ComponentTypeId = 1;
            var com2 = new ComponentType();
            com2.ComponentName = "hedsadasj";
            com2.ComponentTypeId = 4;
            //requires list of all component types
            ViewBag.list = _typeRepo.GetAll().ToList<ComponentType>();
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ComponentViewModel model)
        {
            //needs to create a category and create the binding to the chosen componenttypes

            using (var db = new ApplicationDbContext())
            {
                var com = new Component();
                com.ComponentNumber = model.ComponentNumber;
                com.SerialNo = model.SerialNo;
                com.ComponentTypeId = model.ComponentTypeId;
                db.Components.Add(com);
                db.SaveChanges();

            }

            return RedirectToAction("", "component", new { area = "" });
        }

        [HttpGet("[controller]/[action]/{id}")]
        public IActionResult Show(int id)
        {
            //needs to find a component with its componenttype names in the list
            var comp = new Component();


            var com = new ComponentType();


            using (var db = new ApplicationDbContext())
            {

               comp = db.Components.Where(c => c.ComponentId == id).AsNoTracking().First();
               com = db.ComponentTypes.Where(ct => ct.ComponentTypeId == comp.ComponentTypeId).AsNoTracking().First();

            }

            ViewBag.component = comp;

            ViewBag.componentType = com;
            return View();
        }

        [Authorize]
        [HttpGet("[controller]/[action]/{id}")]
        public IActionResult Edit(int id)
        {
            //needs to find a component with its componenttype
            var compVM = new ComponentViewModel();
            var comp = new Component();

            using (var db = new ApplicationDbContext())
            {
                comp = db.Components.Where(c => c.ComponentId == id).AsNoTracking().First();
            }

            compVM.ComponentId = comp.ComponentId;
            compVM.ComponentNumber = comp.ComponentNumber;
            compVM.ComponentTypeId = comp.ComponentTypeId;
            compVM.SearchTerm = comp.SearchTerm;
            compVM.SerialNo = comp.SerialNo;
            

            //var com = new ComponentType();
            //com.ComponentName = "hej";
            //com.ComponentTypeId = 1;
            //var com2 = new ComponentType();
            //com2.ComponentName = "hedsadasj";
            //com2.ComponentTypeId = 2;
            //cat.ComponentType = com;
            ViewBag.list = _typeRepo.GetAll().ToList();
            return View(compVM);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Update(Component model)
        {

            var comp = new Component();

            using (var db = new ApplicationDbContext())
            {
                comp = db.Components.Where(c => c.ComponentId == model.ComponentId).AsNoTracking().First();
            }

            if (comp != null)
            {
                comp.ComponentNumber = model.ComponentNumber;
                comp.ComponentTypeId = model.ComponentTypeId;
                comp.SerialNo = model.SerialNo;
            }

            using (var db = new ApplicationDbContext())
            {
                db.Entry(comp).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("", "component", new { area = "" });
        }

        [Authorize]
        [HttpGet("[controller]/[action]/{id}")]
        public IActionResult Delete(int id)
        {
            //delete category and its bindings to componttypes
            using (var db = new ApplicationDbContext())
            {
                var del = new Component { ComponentId = id };
                db.Components.Remove(del);
                db.SaveChanges();
            }


            return RedirectToAction("", "component", new { area = "" });
        }

        public IActionResult Search(ComponentViewModel model){

            var cat = new ComponentViewModel();
            cat.ComponentNumber = 1231345454;
            //needs list of all component
            cat.SerialNo = "12a3ds12a";
            cat.ComponentId = 2;
            var com = new ComponentType();
            com.ComponentName = "hej";
            com.ComponentTypeId = 1;
            //cat.ComponentType = com;
            //show filtered list of Components in shape of a viewmodel


            using (var db = new ApplicationDbContext())
            {
                // ViewBag.list = db.Components.ToList<Component>();

                if (model.SearchTerm!=null) { 
                    var tempS = db.Components.Where(c => c.ComponentNumber.ToString().Contains(model.SearchTerm));

                ViewBag.list = tempS.ToList<Component>();
            }

                else
                    ViewBag.list = db.Components.ToList<Component>();
            }

            

            return View("Index", model);
        }

    }
}
