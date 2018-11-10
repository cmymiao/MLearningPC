using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.BmobModel
{
    public class Resource : BmobTable
    {
        public BmobInt id { get; set; }
        public String name { get; set; }
        public BmobFile program { get; set; }
        public BmobFile experiment { get; set; }
        public BmobFile time { get; set; }
        public String filename { get; set; }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            //读取属性值
            this.id = input.getInt("id");
            this.name = input.getString("name");
            this.program = input.getFile("program");
            this.experiment = input.getFile("experiment");
            this.time = input.getFile("time");
            this.filename = input.getString("filename");
        }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            //写到发送端
            output.Put("id", this.id);
            output.Put("name", this.name);
            output.Put("program", this.program);
            output.Put("experiment", this.experiment);
            output.Put("time", this.time);
            output.Put("filename", this.filename);
        }
    }
}