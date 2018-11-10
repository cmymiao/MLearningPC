using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class User_View          //显示的实体类
    {
        public String username { get; set; }
        public String password { get; set; }
        public String name { get; set; }
        public String identity { get; set; }
        public int firstTime { get; set; }
       
    }
}