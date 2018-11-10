using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class Question_View          //显示的实体类
    {
        public int id { get; set; }
        public int difficulty { get; set; }
        public int totalNum { get; set; }
        public int rightNum { get; set; }
        public String question { get; set; }
        public String did { get; set; }
        public String a { get; set; }
        public String b { get; set; }
        public String c { get; set; }
        public String d { get; set; }
        public String answer { get; set; }
        public int courseId { get; set; }
        public int unitId { get; set; }
        public String knowledgeId { get; set; }
        public String analysis { get; set; }
       
    }
}