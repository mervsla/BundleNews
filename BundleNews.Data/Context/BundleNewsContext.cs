using BundleNews.Data.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BundleNews.Data.Context
{
    public class BundleNewsContext :DbContext
    {
        public DbSet<NewsSource> NewsSource { get; set; }
        public DbSet<NewsContent> NewsContent { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=TCBLGADMCONS021;Initial Catalog=BundleNewsDb;Integrated Security=SSPI;");
        }
    }
}
