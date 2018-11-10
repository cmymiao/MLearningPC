using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class Knowledge_View          //显示的实体类
    {
        public int id { get; set; }
        public String name { get; set; }
        public int courseId { get; set; }
        public int unitId { get; set; }


    }
}