using BundleNews.Data.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BundleNews.Data.Entities.Concrete
{
    public class NewsSource : BaseEntity
    {
        public NewsSource()
        {
            Categories = new List<Category>();
        }
        [Required]
        public string Name { get; set; }
        public string URL { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
