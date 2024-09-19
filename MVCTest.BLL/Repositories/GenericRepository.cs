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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly AppDbContext _dbContext1;
        // هيورثوه مني الديبارتمنت والايمبلويي وهيكون عندهم برايفت 
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext1 = dbContext;
        }

        public void Add(T item)
        {
            // _dbContext1.Set<T>().Add(item); or remove set
            _dbContext1.Add(item);
            //return _dbContext1.SaveChanges();
        }

        public void Delete(T item)
        {
            _dbContext1.Remove(item);
            // return _dbContext1.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) _dbContext1.Employees.Include(E => E.Department).AsNoTracking().ToList();
            }
            else
            {
                // لو مش بكلم ايمبلويي اصلا الريتيرن هترجع عادي زي ماهي
                return _dbContext1.Set<T>().AsNoTracking().ToList();
            }
        }

        public T GetById(int id)
        {
            var T = _dbContext1.Set<T>().Find(id);
            return T;
        }

        public void Update(T item)
        {
            _dbContext1.Set<T>().Update(item);
            //return _dbContext1.SaveChanges();
        }
    }
}
