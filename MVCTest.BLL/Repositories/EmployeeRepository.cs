using Microsoft.EntityFrameworkCore;
using MVCTest.BLL.Interfaces;
using MVCTest.DAL.Data;
using MVCTest.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTest.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeReopsitory
    {
        //private readonly AppDbContext _dbContext1;
        // الاوبجكت ده موجود عشان لو الفانكشن بتاعت جيت باي ادريس ديه اقدر اكلم الداتا بيز فيها 
        // بعد ما خليته هناك برايفت بروتيكتيد في ال جينيريك ريبو 
        // اقدر هنا ميكونش عندي ريبريزنتشن ليه لاني ورثته 
        public EmployeeRepository(AppDbContext dbContext):base(dbContext)
        {
            //_dbContext1 = dbContext;
        }

        public IQueryable GetEmployeeByAddress(string address)
        {
           return _dbContext1.Employees.Where(E=>E.Address.ToLower().Contains(address.ToLower()));
           //هنا انا حولت الادريس اللي جاي من الداتا بيز ل تولور وقولتله لو الادريس اللي في الداتا بيز
           // بيكونتين الادريس اللي انا باعتهولك ده رجعهولي 
           
        }


    }
}
