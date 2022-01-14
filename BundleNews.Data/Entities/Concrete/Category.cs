using BundleNews.Data.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BundleNews.Data.Entities.Concrete
{
    public class Category : BaseEntity
    {
        public Category()
        {
            NewsContents = new List<NewsContent>();
        }
        public string Link { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Range(0, Double.PositiveInfinity)]
        public int NewsSourceId { get; set; }
        public virtual NewsSource NewsSource { get; set; }
        public virtual ICollection<NewsContent> NewsContents { get; set; }
    }
}
