using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVCTest.BLL.Interfaces;
using MVCTest.DAL.Models;
using System;

namespace MVCTest.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // DepartmentController is a Controller 
        // DepartmentController has a DepartmentRepository ==> Association ==> Composition  
        // private DepartmentRepository _departmetRepository; عشان ده ميكونش ب null  
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentRepository departmentRepository, IWebHostEnvironment env)
        {
            _departmentRepository = departmentRepository;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index() // GetAll
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var count = _departmentRepository.Add(department);
                if (count > 0)
                {
                    // لو القسم اتضاف والكاونت كان اكبر من زيرو وديني على الفيو بتاع الاندكس اللي
                    // هو هيعرضلي كل الاقسام اللي عندي 
                    return RedirectToAction("Index");
                }

            }
            // لو متضافش ، رجعلي نفس الفيو بتاعت الكريشن اللي هي الفورم بتاعت الداتا اللي هي انا فيها بس هرميلك الموديل عشان الداتا اللي انت دخلتها 
            // متضيعش واخليك تدخل كل اللي انت دخلته من تاني 
            // مش بس اللي مش فاليد 
            return View(department);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();    // 400
            }

            var department = _departmentRepository.GetById(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            return View(viewName, department); // 404

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if(!id.HasValue)
            //{
            //    return BadRequest();
            //}
            //var department = _departmentRepository.GetById(id.Value);
            //if (department == null)
            //    return NotFound();
            //return View(department);
            return Details(id, "Edit");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(department);
            }
            // ممكن وهو بيعمل ابديت في الداتابيز يحصل اي مشكلة سواء بعت اي دي غلط او حصل اي اكسبشن في السيكوال 
            // لذلك هنجرب نحط الموضوع ده في تراي كاتش 
            try
            {
                // هنا اخدت الكود اللي ممكن يرمي اكسبشن 
                _departmentRepository.Update(department);
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, Ex.Message);
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Occured during Update Department");
                }
                return View(department); // بقوله لو حصل اكسبشن يروح يبعتلي نفس الداتا اللي في الديبارتمنت 
                                         // عشان ميمسحهاش يعني ويخليني اعيد من الاول 

            }
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Department department)
        {
            try
            {
                // هنا اخدت الكود اللي ممكن يرمي اكسبشن 
                _departmentRepository.Delete(department);
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, Ex.Message);
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Occured during Delete Department");
                }
                return View(department); // بقوله لو حصل اكسبشن يروح يبعتلي نفس الداتا اللي في الديبارتمنت 
                                         // عشان ميمسحهاش يعني ويخليني اعيد من الاول 

            }
        }
    }
}
