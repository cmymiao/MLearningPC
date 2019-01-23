using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class Resource_View
    {
        public int id { get; set; }
        public BmobFile file { get; set; }
        public String fileUrl { get; set; }
        public String fileName { get; set; }
        public String type { get; set; }
        public int courseId { get; set; }
        public int unitId { get; set; }
        public int knowledgeId { get; set; }
    }
}