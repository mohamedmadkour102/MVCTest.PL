using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MVCTest.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTest.DAL.Data.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Fluent Api's
            builder.Property(D => D.Id).UseIdentityColumn(10, 10);
            
            builder.HasMany(D => D.Employees).WithOne(D => D.Department).OnDelete(DeleteBehavior.Cascade);
        }
    }
}