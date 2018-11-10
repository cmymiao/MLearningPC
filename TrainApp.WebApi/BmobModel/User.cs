using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.BmobModel
{
    public class User : BmobUser
    {
        public String name { get; set; }
        public String nickname { get; set; }
        public String mobilePhoneNumber { get; set; }

        public String classId { get; set; }
        public String identity { get; set; }

        public BmobInt firstTime { get; set; }
        public BmobInt courseId { get; set; }

        public User() { }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.name = input.getString("name");
            this.nickname = input.getString("nickname");
            this.mobilePhoneNumber = input.getString("mobilePhoneNumber");
            this.email = input.getString("email");
            this.classId = input.getString("classId");

            this.identity = input.getString("identity");
            this.username = input.getString("username");
            this.password = input.getString("password");
            this.firstTime = input.getInt("firstTime");
            this.courseId = input.getInt("courseId");
        }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("name", this.name);
            output.Put("nickname", this.nickname);
            output.Put("mobilePhoneNumber", this.mobilePhoneNumber);
            output.Put("email", this.email);
            output.Put("classId", this.classId);
            output.Put("username", this.username);
            output.Put("password", this.password);
            output.Put("firstTime", this.firstTime);
            output.Put("identity", this.identity);
            output.Put("courseId", this.courseId);
        }
    }
}