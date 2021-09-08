using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearningWebsite.Models
{
    public class EnrollClassModel
    {
        public int ClassId { get; set; }
        public IEnumerable<SelectListItem> Classes { get; set; }
    }
}