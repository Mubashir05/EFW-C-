﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3
{
    public class ModelContext : DbContext
    {
        public ModelContext() : base("name=con") { }

        public DbSet<Employee> EmpList { get; set; }


    }
}
