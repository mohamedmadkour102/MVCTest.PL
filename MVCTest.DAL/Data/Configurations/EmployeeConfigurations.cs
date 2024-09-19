using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVCTest.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCTest.DAL.Data.Configurations
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Salary).HasColumnType("decimal(18,2)");

            builder.Property(E=> E.Gender )
                   .HasConversion(
                (Gender) => Gender.ToString() ,
                (GenderAsString) => (Gender) Enum.Parse(typeof(Gender),GenderAsString,true)
                
                );
            builder.Property(E => E.Name).IsRequired(true).HasMaxLength(50);
        }
    }
}
