using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.BmobModel
{
    public class Unit : BmobTable
    {
        public BmobInt id { get; set; }
        public String name { get; set; }
        public BmobInt courseId { get; set; }
        

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            //读取属性值
            this.id = input.getInt("id");
            this.name = input.getString("name");
            this.courseId = input.getInt("courseId");
        }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            //写到发送端
            output.Put("id", this.id);
            output.Put("name", this.name);
            output.Put("courseId", this.courseId);
        }
    }
}