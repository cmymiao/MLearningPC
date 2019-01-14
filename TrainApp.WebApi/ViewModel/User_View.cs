using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class User_View          //显示的实体类
    {
        public String objectId { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String name { get; set; }
        public String identity { get; set; }
        public int firstTime { get; set; }
        public String nickname { get; set; }
        public String mobilePhoneNumber { get; set; }
        public String email { get; set; }
        public String classId { get; set; }
        public int courseId { get; set; }

    }
}