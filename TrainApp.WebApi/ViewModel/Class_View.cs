using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class Class_View
    {
        public String id { get; set; }
        public String name { get; set; }
        public String schedule { get; set; }
        public int courseId { get; set; }
        public String tId { get; set; }
        public String term { get; set; }
    }
}