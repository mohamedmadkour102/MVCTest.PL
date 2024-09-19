using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVCTest.BLL.Interfaces;

using MVCTest.DAL.Models;
using MVCTest.PL.Helpers;
using MVCTest.PL.ViewModels;
using System;
using System.Collections.Generic;

namespace MVCTest.PL.Controllers
{
    public class EmployeeController : Controller
    {
        // EmployeeController is a Controller 
        // EmployeeController has a EmployeeRepository ==> Association ==> Composition  
        // private EmployeeRepository _employeeRepository; عشان ده ميكونش ب null  
        //private readonly IEmployeeReopsitory _EmployeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        // private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IUnitOfWork unitOfWork/*IEmployeeReopsitory employeerepository*/, IWebHostEnvironment env  , IMapper mapper/*IDepartmentRepository departmentRepository*/)
        {
            //_EmployeeRepository = employeerepository;
            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
            //   _departmentRepository = departmentRepository;
        }

        // [HttpGet]
        // طالاما مش عندي غير اكشن واحد اسمه اندكس ف هو الديفولت بتاعه بيكون جيت 
        // وطالاما الفورم بيتعامل معاه ف هيتعامل معاه على اساس انه بوست ، لانه الديفولت بتاع 
        // الفورم بوست 
        public IActionResult Index(string searchInp) // GetAll
        {                                
            if (string.IsNullOrEmpty(searchInp))
            {
                var Employees = _unitOfWork.EmployeeReopsitory.GetAll(); 
                var mappedVM = _mapper.Map<IEnumerable<Employee>,IEnumerable<EmployeeViewModel>>(Employees);
                return View(mappedVM);
            }
            else {
                var Employees = _unitOfWork.EmployeeReopsitory.GetEmployeeByName(searchInp.ToLower());
                var mappedVM = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>((IEnumerable<Employee>)Employees);

                return View(mappedVM);
            }
            // Extra Info 
            // Binding Through View's Dictionary : transfer data from action to view
            // 1. ViewData 
            //ViewData["Massage"] = "Hello ViewData";

            // 2. ViewBag
            //ViewBag.Massage = "Hello ViewBag";

      
        }
        [HttpGet]
        public IActionResult Create()
        {

           // ViewData["Department"]=_departmentRepository.GetAll();
            //ممكن تستخدم الفيوباج
            //ViewBag.Department = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel EmployeeVM)
        {
            if (ModelState.IsValid)
            {
                EmployeeVM.ImageName = DocumentSetting.UploadFile(EmployeeVM.Image, "Images"); 
                var mappedVM = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                _unitOfWork.EmployeeReopsitory.Add(mappedVM);
                var count = _unitOfWork.Complete();
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
            return View(EmployeeVM);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();    // 400
            }

            var Employee = _unitOfWork.EmployeeReopsitory.GetById(id.Value);
           // ViewData["Department"]= _departmentRepository.GetAll();

            if (Employee == null)
            {
                return NotFound();
            }
            var EmployeeVM = _mapper.Map<Employee, EmployeeViewModel>(Employee);

            return View(viewName, EmployeeVM);
             // 404

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
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel EmployeeVM)
        {
            if (id != EmployeeVM.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(EmployeeVM);
            }
            // ممكن وهو بيعمل ابديت في الداتابيز يحصل اي مشكلة سواء بعت اي دي غلط او حصل اي اكسبشن في السيكوال 
            // لذلك هنجرب نحط الموضوع ده في تراي كاتش 
            try
            {
                EmployeeVM.ImageName = DocumentSetting.UploadFile(EmployeeVM.Image, "Images");
                var mappedVM = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                _unitOfWork.EmployeeReopsitory.Update(mappedVM);
                _unitOfWork.Complete();


                // هنا اخدت الكود اللي ممكن يرمي اكسبشن 

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
                return View(EmployeeVM); // بقوله لو حصل اكسبشن يروح يبعتلي نفس الداتا اللي في الديبارتمنت 
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
        public IActionResult Delete(EmployeeViewModel EmployeeVM)
        {
            try
            {
                var mappedVM = _mapper.Map<EmployeeViewModel,Employee>(EmployeeVM); 
                _unitOfWork.EmployeeReopsitory.Update(mappedVM);
                _unitOfWork.Complete();
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
                return View(EmployeeVM); // بقوله لو حصل اكسبشن يروح يبعتلي نفس الداتا اللي في الديبارتمنت 
                                         // عشان ميمسحهاش يعني ويخليني اعيد من الاول 

            }
        }
    }
}
