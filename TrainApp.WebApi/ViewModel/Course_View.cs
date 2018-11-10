using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class Course_View          //显示的实体类
    {
        public int id { get; set; }
        public String name { get; set; }
        public BmobFile time { get; set; }
        public String times { get; set; }
        public String timeu { get; set; }
        public BmobFile program { get; set; }
        public String programs { get; set; }
        public String programu { get; set; }
        public BmobFile experiment { get; set; }
        public String experiments { get; set; }
        public String experimentu { get; set; }
        
    }
}