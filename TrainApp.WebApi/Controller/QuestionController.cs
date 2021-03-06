﻿using cn.bmob.io;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TrainApp.WebApi.BmobModel;
using TrainApp.WebApi.ViewModel;

namespace TrainApp.WebApi
{
    [Route("ShowQuestion")]                 //定义访问该控制器中的路由（地址）名称
    [AllowAnonymous]                    //定义使用匿名访问方式访问路由地址
    public class QuestionController : BmobBaseForm          //需继承BmobBaseForm
    {
        public Question question = new Question();
        public List<Question> questionList = new List<Question>();
        public List<Question_View> qList = new List<Question_View>();

        public List<Question_View> questionSortList = new List<Question_View>();


        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object Get()             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            QuestionInfo();
            return ResultToJson.toJson(qList);      //返回数据需要json格式

        }

        public void QuestionInfo()              //查询全部题目的函数
        {
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String id = cookie1["CourseId"];
            int courseId = int.Parse(id);
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("courseId", courseId);
            query.OrderBy("id");
            var future = Bmob.FindTaskAsync<Question>("Question", query); 
            try
            {
                questionList = future.Result.results;
                foreach (var q in questionList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Question_View question_view = new Question_View();
                    question_view.objectId = q.objectId;
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
            }
            catch { }
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
        public object PostAddQuestion([FromBody]Question_View questionInfo)
        {
            Question question = new Question();
            question.id = questionInfo.id;
            question.question = questionInfo.question;
            question.a = questionInfo.a;
            question.b = questionInfo.b;
            question.c = questionInfo.c;
            question.d = questionInfo.d;
            question.a = questionInfo.a;
            question.answer = questionInfo.answer;
            question.analysis = questionInfo.analysis;
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String Id = cookie1["CourseId"];
            int courseId = int.Parse(Id);
            question.courseId = courseId;
            question.unitId = questionInfo.unitId;
            question.knowledgeId = questionInfo.knowledgeId;
            question.difficulty = questionInfo.difficulty;
            question.rightNum = questionInfo.rightNum;
            question.totalNum = questionInfo.totalNum;
            var future = Bmob.CreateTaskAsync(question);
            String id = future.Result.objectId;
            return id;
        }



        [Route("DeleteQuestion")]
        [HttpGet]
        public object GetDeleteQuestion(int id, int courseId)
        {
            //两个条件的查询
            var query1 = new BmobQuery();
            query1.WhereEqualTo("id", id);
            var query2 = new BmobQuery();
            query1.WhereEqualTo("courseId", courseId);
            //And操作
            query1 = query1.And(query2);
            var future = Bmob.FindTaskAsync<Question>("Question", query1);
            try
            {
                var a = future.Result;
                String objectId = future.Result.results[0].objectId;
                var future1 = Bmob.DeleteTaskAsync("Question", objectId);
                return future1.Result.msg;
            }
            catch
            {
                return "获取失败";
            }

        }

        [Route("UploadQuestion")]
        [HttpPost]
        public object PostUploadQuestion([FromBody]List<Question_View> question1)
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

        //[Route("QueryCourseQuestion")]
        //[HttpGet]
        //public object GetQueryCourseQuestion(int courseId)
        //{
        //    var query = new BmobQuery();
        //    query.Limit(300);
        //    query.WhereEqualTo("courseId", courseId);
        //    var future = Bmob.FindTaskAsync<Question>("Question", query);
        //    questionList = future.Result.results;
        //    foreach (var q in questionList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
        //    {
        //        Question_View question_view = new Question_View();
        //        question_view.id = q.id.Get();
        //        question_view.difficulty = q.difficulty.Get();
        //        question_view.totalNum = q.totalNum.Get();
        //        question_view.rightNum = q.rightNum.Get();
        //        question_view.question = q.question;
        //        question_view.a = q.a;
        //        question_view.b = q.b;
        //        question_view.c = q.c;
        //        question_view.d = q.d;
        //        question_view.answer = q.answer;
        //        question_view.analysis = q.analysis;
        //        question_view.courseId = q.courseId.Get();
        //        question_view.unitId = q.unitId.Get();
        //        question_view.knowledgeId = q.knowledgeId;
        //        qList.Add(question_view);

        //    }
        //    return ResultToJson.toJson(qList);

        //}
        [Route("QueryUnitQuestion")]
        [HttpGet]
        public object GetQueryUnitQuestion(int unitId)
        {
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String Id = cookie1["CourseId"];
            int courseId = int.Parse(Id);
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("courseId", courseId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("unitId", unitId);
            query = query.And(query1);
            var future = Bmob.FindTaskAsync<Question>("Question", query);
            try
            {
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
                return ResultToJson.toJson(qList);
            }
            catch
            {
                return "获取失败";
            }
        }

        [Route("UpdateQuestion")]
        [HttpPost]
        public object PostUpdateQuestion([FromBody]List<Question_View> questionsInfo)
        {
            String result = "";
            int count = 0;
            int length = questionsInfo.Count;
            for (int i = 0; i < questionsInfo.Count; i++)
            {
                Question question = new Question();
                question.objectId = questionsInfo[i].objectId;
                question.id = questionsInfo[i].id;
                question.question = questionsInfo[i].question;
                question.a = questionsInfo[i].a;
                question.b = questionsInfo[i].b;
                question.c = questionsInfo[i].c;
                question.d = questionsInfo[i].d;
                question.a = questionsInfo[i].a;
                question.answer = questionsInfo[i].answer;
                question.analysis = questionsInfo[i].analysis;
                question.courseId = questionsInfo[i].courseId;
                question.unitId = questionsInfo[i].unitId;
                question.knowledgeId = questionsInfo[i].knowledgeId;
                question.difficulty = questionsInfo[i].difficulty;
                question.totalNum = questionsInfo[i].totalNum;
                question.rightNum = questionsInfo[i].rightNum;
                questionList.Add(question);
            }
            
            for (int i = 0; i < length; i++)
            {
                var query1 = new BmobQuery();
                query1.WhereEqualTo("id", questionList[i].id);
                var query2 = new BmobQuery();
                query1.WhereEqualTo("courseId", questionList[i].courseId);
                //And操作
                query1 = query1.And(query2);
                var future = Bmob.FindTaskAsync<Question>("Question", query1);
                String objectId = future.Result.results[0].objectId;
                var future1 = Bmob.UpdateTaskAsync("Question", objectId, questionList[i]);
                try
                {
                    String a = future1.Result.updatedAt;
                    count++;
                    if (count == length)
                    {
                        result = "success";
                    }
                }
                catch
                {
                    result = "fail";
                }
            }
            return result;

        }

        //全部单元题目答题情况统计
        [Route("AllUnitQuestionStatistic")]
        [HttpGet]
        public object GetQuestionStatistic()
        {
            var query = new BmobQuery();
            query.Limit(300);
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String id = cookie1["CourseId"];
            int courseId = int.Parse(id);
            query.WhereEqualTo("courseId", courseId);
            //query.OrderByDescending("rightNum");
            var future = Bmob.FindTaskAsync<Question>("Question", query);
            try
            {
                questionList = future.Result.results;
                foreach (var q in questionList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Question_View question_view = new Question_View();
                    question_view.id = q.id.Get();
                    question_view.question = q.question;
                    question_view.totalNum = q.totalNum.Get();
                    question_view.rightNum = q.rightNum.Get();

                    if (question_view.totalNum != 0)
                    {
                        question_view.accuracy = 100 * question_view.rightNum / question_view.totalNum;
                    }
                    qList.Add(question_view);
                }

                questionSortList = qList.OrderByDescending(s => s.accuracy).ToList();

                return ResultToJson.toJson(questionSortList);
            }
            catch
            {
                return "获取失败";
            }
        }

        //单元题目答题情况统计
        [Route("QuestionStatistic")]
        [HttpGet]
        public object GetQuestionStatistic(int unitId)
        {
            var query = new BmobQuery();
            query.Limit(300);
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String id = cookie1["CourseId"];
            int courseId = int.Parse(id);
            query.WhereEqualTo("courseId", courseId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("unitId", unitId);
            query = query.And(query1);
            //query.OrderByDescending("rightNum");
            var future = Bmob.FindTaskAsync<Question>("Question", query);
            try
            {
                questionList = future.Result.results;
                foreach (var q in questionList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Question_View question_view = new Question_View();
                    question_view.id = q.id.Get();
                    question_view.question = q.question;
                    question_view.totalNum = q.totalNum.Get();
                    question_view.rightNum = q.rightNum.Get();

                    if (question_view.totalNum != 0)
                    {
                        question_view.accuracy = 100 * question_view.rightNum / question_view.totalNum;
                    }
                    qList.Add(question_view);
                }

                questionSortList = qList.OrderByDescending(s => s.accuracy).ToList();

                return ResultToJson.toJson(questionSortList);
            }
            catch
            {
                return "获取失败";
            }
        }

        //查看试卷详情
        [Route("ShowDetails")]
        [HttpGet]
        public object GetShowDetails(String examQuestionList)
        {
            string[] results = examQuestionList.Split(new[] { ';' });
            int[] examQuestion = Array.ConvertAll<string, int>(results, s => int.Parse(s));

            for (int i = 0; i < examQuestion.Length; i++)
            {
                var query = new BmobQuery();
                query.WhereEqualTo("id", examQuestion[i]);
                HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
                String Id = cookie1["CourseId"];
                int courseId = int.Parse(Id);
                var query2 = new BmobQuery();
                query2.WhereEqualTo("courseId", courseId);
                query = query.And(query2);
                query.OrderBy("id");
                var future = Bmob.FindTaskAsync<Question>("Question", query);
                try
                {
                    questionList = future.Result.results;
                    foreach (var q in questionList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                    {
                        Question_View question_view = new Question_View();
                        question_view.id = q.id.Get();
                        question_view.difficulty = q.difficulty.Get();
                        question_view.question = q.question;
                        question_view.courseId = q.courseId.Get();
                        question_view.unitId = q.unitId.Get();
                        question_view.knowledgeId = q.knowledgeId;
                        qList.Add(question_view);
                    }
                }
                catch
                {
                    return "获取失败";
                }
            }
            return ResultToJson.toJson(qList);
        }

        //试卷题目修改
        [Route("SelectedQuestion")]
        [HttpGet]
        public object GetSelectedQuestion(int unitId,String knowledgeId,int difficulty)
        {
            var query = new BmobQuery();
            query.Limit(300);
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String Id = cookie1["CourseId"];
            int courseId = int.Parse(Id);
            query.WhereEqualTo("courseId", courseId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("unitId", unitId);
            var query2 = new BmobQuery();
            query2.WhereEqualTo("knowledgeId", knowledgeId);
            var query3 = new BmobQuery();
            query3.WhereEqualTo("difficulty", difficulty);
            query2 = query2.And(query3);
            query1 = query1.And(query2);
            query = query.And(query1);
            var future = Bmob.FindTaskAsync<Question>("Question", query);
            try
            {
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
                return ResultToJson.toJson(qList);
            }
            catch
            {
                return "获取失败";
            }
        }

    }//结束括号
}