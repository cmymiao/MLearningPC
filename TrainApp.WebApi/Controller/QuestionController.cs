using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TrainApp.WebApi.BmobModel;
using TrainApp.WebApi.ViewModel;

namespace TrainApp.WebApi
{
    [Route("Question")]                 //定义访问该控制器中的路由（地址）名称
    [AllowAnonymous]                    //定义使用匿名访问方式访问路由地址
    public class QuestionController : BmobBaseForm          //需继承BmobBaseForm
    {
        public Question question = new Question();
        public List<Question> questionList = new List<Question>();
        public List<Question_View> qList = new List<Question_View>();

        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object Get()             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            QuestionInfo();
            return ResultToJson.toJson(qList);      //返回数据需要json格式

        }

        public void QuestionInfo()              //查询函数
        {
            var query = new BmobQuery();
            query.Limit(300);
            var future = Bmob.FindTaskAsync<Question>("Question", query);
            questionList = future.Result.results;
            foreach (var q in questionList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
            {
                Question_View question_view = new Question_View();
                question_view.id = q.id.Get();
                question_view.difficulty = q.difficulty.Get();
                question_view.totalNum = q.totalNum.Get();
                question_view.rightNum = q.rightNum.Get();
                question_view.question = q.question;
                question_view.a = q.a;
                question_view.b = q.b;
                question_view.c = q.c;
                question_view.d = q.d;
                question_view.answer = q.answer;
                question_view.analysis = q.analysis;
                question_view.courseId = q.courseId.Get();
                question_view.unitId = q.unitId.Get();
                question_view.knowledgeId = q.knowledgeId;
                qList.Add(question_view);
            }
            //如果需要对其中一行数据进行处理，可以记录每以条记录的object值，方便对Bmob数据库进行操作。
        }




        List<Question_View> quesList = new List<Question_View>();
        [Route("GetQuestion")]
        [HttpPost]
       
            public object Post([FromBody]List<Question_View> question)
        {

            quesList.AddRange(question);
            return ResultToJson.toJson(question);
        }
        

        [Route("AddQuestion")]
        [HttpPost]
        public object PostAddQuestion([FromBody]List<Question_View> question1)
        {
            String a = "";
            List<String> list = new List<String>();
            quesList.AddRange(question1);
            int length = quesList.Count;
            for (int i = 0; i < quesList.Count; i++)
            {
                Question question = new Question();
                question.id = quesList[i].id;
                question.question = quesList[i].question;
                question.a = quesList[i].a;
                question.b = quesList[i].b;
                question.c = quesList[i].c;
                question.d = quesList[i].d;
                question.a = quesList[i].a;
                question.answer = quesList[i].answer;
                question.analysis = quesList[i].analysis;
                question.courseId = quesList[i].courseId;
                question.unitId = quesList[i].unitId;
                question.knowledgeId = quesList[i].knowledgeId;
                question.difficulty = quesList[i].difficulty;
                question.totalNum = quesList[i].totalNum;
                question.rightNum = quesList[i].rightNum;

                var future = Bmob.CreateTaskAsync(question);
                try
                {
                    String id = future.Result.objectId;
                    list.Add(id);
                }
                catch
                {
                    //没成功的会跳转到这里
                }

            }
            if (quesList.Count == list.Count)
            {
                a = "上传成功";
            }
            else
            {
                a = "上传失败";
            }
            return a;
        }




    }
}