﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.ViewModel
{
    public class Requirement
    {
        public int courseId { get; set; }
        public String difficulty { get; set; }
        public String unitWeight { get; set; }
    }
}