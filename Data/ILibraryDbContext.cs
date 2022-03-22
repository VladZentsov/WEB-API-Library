using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface ILibraryDbContext
    {
        int SaveChanges();
        DbSet<T> Set<T>() where T : class;
    }
}
