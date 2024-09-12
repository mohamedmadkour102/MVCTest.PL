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

        public int Add(T item)
        {
            // _dbContext1.Set<T>().Add(item); or remove set
            _dbContext1.Add(item);
            return _dbContext1.SaveChanges();
        }

        public int Delete(T item)
        {
            _dbContext1.Remove(item);
            return _dbContext1.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbContext1.Set<T>().AsNoTracking().ToList(); // لاني عايز ارجعهم بس عملت نو تراكينج 
        }

        public T GetById(int id)
        {
            var T = _dbContext1.Set<T>().Find(id);
            return T;
        }

        public int Update(T item)
        {
            _dbContext1.Set<T>().Update(item);
            return _dbContext1.SaveChanges();
        }
    }
}
