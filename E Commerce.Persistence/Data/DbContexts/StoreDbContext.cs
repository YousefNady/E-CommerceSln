using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.DbContexts
{
    public class StoreDbContext : DbContext
    {
        // dependency injection
        // clr will inject the object 
        // + need to add that services in program.cs
        public StoreDbContext(DbContextOptions<StoreDbContext> Options) : base(Options)  
        {
            
        }



    }
}
