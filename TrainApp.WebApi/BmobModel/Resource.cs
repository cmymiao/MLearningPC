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
        public BmobFile file { get; set; }
        public String type { get; set; }
        public BmobInt courseId { get; set; }
        public BmobInt unitId { get; set; }
        public BmobInt knowledgeId { get; set; }

        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.id = input.getInt("id");
            this.file = input.getFile("file");
            this.type = input.getString("type");
            this.courseId = input.getInt("courseId");
            this.unitId = input.getInt("unitId");
            this.knowledgeId = input.getInt("knowledgeId");
        }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("id", this.id);
            output.Put("file", this.file);
            output.Put("type", this.type);
            output.Put("courseId", this.courseId);
            output.Put("unitId", this.unitId);
            output.Put("knowledgeId", this.knowledgeId);
        }
    }
}