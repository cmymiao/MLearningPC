using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class Examination_View          //显示的实体类
    {
        public String objectId { get; set; }
        public String name { get; set; }
        public String questionList { get; set; }
        public int id { get; set; }
        public double difficulty { get; set; }
        public int courseId { get; set; }
        //public int[] examQuestion { get; set;}
    }
}