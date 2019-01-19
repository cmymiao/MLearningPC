using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class Feedback_View          //显示的实体类
    {
        public String objectId { get; set; }
        public String username { get; set; }
        public String nickname { get; set; }
        public int totalNum { get; set; }
        public int rightNum { get; set; }
        public int unitId { get; set; }
        public int courseId { get; set; }
        public String classId { get; set; }
        public int accuracy { get; set; }
    }
}