using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVCTest.BLL.Interfaces;
using MVCTest.DAL.Models;
using MVCTest.PL.ViewModels;
using System;
using System.Collections.Generic;

namespace MVCTest.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // DepartmentController is a Controller 
        // DepartmentController has a DepartmentRepository ==> Association ==> Composition  
        // private DepartmentRepository _departmetRepository; عشان ده ميكونش ب null  
        // private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork/*IDepartmentRepository departmentRepository*/, IWebHostEnvironment env , IMapper mapper)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index() // GetAll
        {

            var departments = _unitOfWork.DepartmentRepository.GetAll();
            var mappedVM = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var mappedVM = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                 _unitOfWork.DepartmentRepository.Add(mappedVM);

                var count = _unitOfWork.Complete();
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
            return View(departmentVM);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();    // 400
            }

            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            var departmentVM = _mapper.Map<Department, DepartmentViewModel>(department);

            return View(viewName, departmentVM);

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
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            // ممكن وهو بيعمل ابديت في الداتابيز يحصل اي مشكلة سواء بعت اي دي غلط او حصل اي اكسبشن في السيكوال 
            // لذلك هنجرب نحط الموضوع ده في تراي كاتش 
            try
            {
                var mappedVM = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                // هنا اخدت الكود اللي ممكن يرمي اكسبشن 
                _unitOfWork.DepartmentRepository.Update(mappedVM);
                _unitOfWork.Complete();
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
                return View(departmentVM); // بقوله لو حصل اكسبشن يروح يبعتلي نفس الداتا اللي في الديبارتمنت 
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
        public IActionResult Delete(DepartmentViewModel departmentVM)
        {
            try
            {
                var mappedVM = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                // هنا اخدت الكود اللي ممكن يرمي اكسبشن 
                _unitOfWork.DepartmentRepository.Delete(mappedVM);
                _unitOfWork.Complete();
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
                return View(departmentVM); // بقوله لو حصل اكسبشن يروح يبعتلي نفس الداتا اللي في الديبارتمنت 
                                         // عشان ميمسحهاش يعني ويخليني اعيد من الاول 

            }
        }
    }
}
