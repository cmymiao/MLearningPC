using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.BmobModel
{
    public class Class : BmobTable
    {
        public String id { get; set; }
        public String name { get; set; }
        public String schedule { get; set; }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.id = input.getString("id");
            this.name = input.getString("name");
            this.schedule = input.getString("schedule");
        }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("id", this.id);
            output.Put("name", this.name);
            output.Put("schedule", this.schedule);
        }
    }
}