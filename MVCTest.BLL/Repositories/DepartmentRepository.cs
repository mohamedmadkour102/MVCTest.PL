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
    public class DepartmentRepository :GenericRepository<Department>,  IDepartmentRepository
    {
       // private readonly AppDbContext _dbContext1;
        public DepartmentRepository(AppDbContext dbContext) : base(dbContext)
        {
           // _dbContext1 = dbContext;
        }

         
    }
}
