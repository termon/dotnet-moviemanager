using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MovieManager.Data.Repositories
{
    public static class DatabaseContextFactory
    {
        
         public static DatabaseContext CreateTestCtx()
         {               
            return new DatabaseContext( 
                new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options
            );         
         }

         public static DatabaseContext CreateCtx()
         {               
            return new DatabaseContext( 
                new DbContextOptionsBuilder<DatabaseContext>()
                    .UseSqlite(@"Data source = movies.db")
                    .LogTo(Console.WriteLine, LogLevel.Information) // remove in production
                    .EnableSensitiveDataLogging().Options           // remove in production
            );         
         }
           
    }
}