using BundleNews.Data.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BundleNews.Data.Entities.Concrete
{
    public class NewsContent : BaseEntity
    {
        public string imageURL { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string NewsContentId { get; set; }
        public DateTime ContentInsertAt { get; set; }
        [Required, Range(0, Double.PositiveInfinity)]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
