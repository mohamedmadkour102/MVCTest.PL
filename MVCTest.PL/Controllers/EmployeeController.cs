using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVCTest.BLL.Interfaces;

using MVCTest.DAL.Models;
using System;

namespace MVCTest.PL.Controllers
{
    public class EmployeeController : Controller
    {
        // EmployeeController is a Controller 
        // EmployeeController has a EmployeeRepository ==> Association ==> Composition  
        // private EmployeeRepository _employeeRepository; عشان ده ميكونش ب null  
        private readonly IEmployeeReopsitory _EmployeeRepository;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeReopsitory employeerepository, IWebHostEnvironment env)
        {
            _EmployeeRepository = employeerepository;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index() // GetAll
        {
            var Employees = _EmployeeRepository.GetAll();
            // Extra Info 
            // Binding Through View's Dictionary : transfer data from action to view
            // 1. ViewData 
            ViewData["Massage"] = "Hello ViewData";

            // 2. ViewBag
            ViewBag.Massage = "Hello ViewBag";

            return View(Employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee Employee)
        {
            if (ModelState.IsValid)
            {
                // 
                var count = _EmployeeRepository.Add(Employee);
                if (count > 0)
                {

                    TempData["Massage"] = "Employee Created Successfuly";
                    // كده وهو رايح يدايركت على الاندكس هياخد ديه معاه 

                    
                }
                else
                {
                    TempData["Massage"] = "An Error Occurred";
                }
                // لو القسم اتضاف والكاونت كان اكبر من زيرو وديني على الفيو بتاع الاندكس اللي
                // هو هيعرضلي كل الاقسام اللي عندي  
                return RedirectToAction("Index");


            }
            // لو متضافش ، رجعلي نفس الفيو بتاعت الكريشن اللي هي الفورم بتاعت الداتا اللي هي انا فيها بس هرميلك الموديل عشان الداتا اللي انت دخلتها 
            // متضيعش واخليك تدخل كل اللي انت دخلته من تاني 
            // مش بس اللي مش فاليد 
            return View(Employee);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();    // 400
            }

            var Employee = _EmployeeRepository.GetById(id.Value);

            if (Employee == null)
            {
                return NotFound();
            }

            return View(viewName, Employee); // 404

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if(!id.HasValue)
            //{
            //    return BadRequest();
            //}
            //var Employee = _EmployeeRepository.GetById(id.Value);
            //if (Employee == null)
            //    return NotFound();
            //return View(Employee);
            return Details(id, "Edit");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee Employee)
        {
            if (id != Employee.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(Employee);
            }
            // ممكن وهو بيعمل ابديت في الداتابيز يحصل اي مشكلة سواء بعت اي دي غلط او حصل اي اكسبشن في السيكوال 
            // لذلك هنجرب نحط الموضوع ده في تراي كاتش 
            try
            {
                // هنا اخدت الكود اللي ممكن يرمي اكسبشن 
                _EmployeeRepository.Update(Employee);
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, Ex.Message);
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Occured during Update Employee");
                }
                return View(Employee); // بقوله لو حصل اكسبشن يروح يبعتلي نفس الداتا اللي في الديبارتمنت 
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
        public IActionResult Delete(Employee Employee)
        {
            try
            {
                // هنا اخدت الكود اللي ممكن يرمي اكسبشن 
                _EmployeeRepository.Delete(Employee);
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, Ex.Message);
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Occured during Delete Employee");
                }
                return View(Employee); // بقوله لو حصل اكسبشن يروح يبعتلي نفس الداتا اللي في الديبارتمنت 
                                         // عشان ميمسحهاش يعني ويخليني اعيد من الاول 

            }
        }
    }
}
