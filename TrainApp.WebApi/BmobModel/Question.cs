using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainApp.WebApi.BmobModel
{
    public class Question:BmobTable         //定义实体类，与数据库表中的字段对应，需继承BmobTable
    {
        public BmobInt id { get; set; }
        public BmobDouble difficulty { get; set; }
        public BmobInt totalNum { get; set; }
        public BmobInt rightNum { get; set; }
        public String question { get; set; }
        public String a { get; set; }
        public String b { get; set; }
        public String c { get; set; }
        public String d { get; set; }
        public BmobFile image { get; set; }
        public String answer { get; set; }
        public String analysis { get; set; }
        public BmobInt courseId { get; set; }
        public BmobInt unitId { get; set; }
        public String knowledgeId { get; set; }
       

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            this.id = input.getInt("id");
            this.difficulty = input.getDouble("difficulty");
            this.totalNum = input.getInt("totalNum");
            this.rightNum = input.getInt("rightNum");
            this.question = input.getString("question");
            this.a = input.getString("a");
            this.b = input.getString("b");
            this.c = input.getString("c");
            this.d = input.getString("d");
            this.image = input.getFile("image");
            this.answer = input.getString("answer");
            this.analysis = input.getString("analysis");
            this.courseId = input.getInt("courseId");
            this.unitId = input.getInt("unitId");
            this.knowledgeId = input.getString("knowledgeId");
            
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            output.Put("id", this.id);
            output.Put("difficulty", this.difficulty);
            output.Put("totalNum", this.totalNum);
            output.Put("rightNum", this.rightNum);
            output.Put("question", this.question);
            output.Put("a", this.a);
            output.Put("b", this.b);
            output.Put("c", this.c);
            output.Put("d", this.d);
            output.Put("image", this.image);
            output.Put("answer", this.answer);
            output.Put("analysis", this.analysis);
            output.Put("courseId", this.courseId);
            output.Put("unitId", this.unitId);
            output.Put("knowledgeId", this.knowledgeId);
         
        }
    }

}
