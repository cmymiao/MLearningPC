using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.BmobModel
{
    public class Feedback : BmobTable
    {
        public String username { get; set; }
        public String nickname { get; set; }
        public BmobInt totalNum { get; set; }
        public BmobInt rightNum { get; set; }
        public BmobInt unitId { get; set; }
        public BmobInt courseId { get; set; }
        public String classId { get; set; }


        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            //读取属性值
            this.username = input.getString("username");
            this.nickname = input.getString("nickname");
            this.totalNum = input.getInt("totalNum");
            this.rightNum = input.getInt("rightNum");
            this.unitId = input.getInt("unitId");
            this.courseId = input.getInt("courseId");
            this.classId = input.getString("classId");
        }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            //写到发送端
            output.Put("username", this.username);
            output.Put("nickname", this.nickname);
            output.Put("totalNum", this.totalNum);
            output.Put("rightNum", this.rightNum);
            output.Put("unitId", this.unitId);
            output.Put("courseId", this.courseId);
            output.Put("classId", this.classId);
        }
    }
}