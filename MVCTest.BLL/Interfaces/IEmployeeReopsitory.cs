using MVCTest.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTest.BLL.Interfaces
{
    public interface IEmployeeReopsitory : IGenericRepository<Employee>
    {
         IQueryable GetEmployeeByAddress(string address);
    }
}
