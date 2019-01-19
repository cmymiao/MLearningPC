using cn.bmob.api;
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
    [Route("ShowFeedback")]                 //定义访问该控制器中的路由（地址）名称
    [AllowAnonymous]                    //定义使用匿名访问方式访问路由地址
    public class FeedbackController : BmobBaseForm          //需继承BmobBaseForm
    {
        public Feedback feedback = new Feedback();
        public List<Feedback> feedbackList = new List<Feedback>();
        public List<Feedback_View> fList = new List<Feedback_View>();

        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object Get()             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            FeedbackInfo();
            return ResultToJson.toJson(fList);      //返回数据需要json格式

        }

        public void FeedbackInfo()              //查询全部题目的函数
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["UserInfoRemember"];
            String username = cookie["username"].ToString();
            var query = new BmobQuery();
            query.WhereEqualTo("tId", username);
            var future = Bmob.FindTaskAsync<Feedback>("Feedback", query);
            try
            {
                feedbackList = future.Result.results;
                foreach (var f in feedbackList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Feedback_View feedback_view = new Feedback_View();
                    feedback_view.objectId = f.objectId;
                    feedback_view.username = f.username;
                    feedback_view.nickname = f.nickname;
                    feedback_view.totalNum = f.totalNum.Get();
                    feedback_view.rightNum = f.rightNum.Get();
                    feedback_view.unitId = f.unitId.Get();
                    feedback_view.classId = f.classId;
                    fList.Add(feedback_view);
                }
            }
            catch
            {

            }
        }
        List<Feedback_View> feedList = new List<Feedback_View>();
        [Route("GetFeedback")]
        [HttpPost]

        public object Post([FromBody]List<Feedback_View> feedback)
        {

            feedList.AddRange(feedback);
            return ResultToJson.toJson(feedback);
        }

        //学生答题情况统计
        [Route("QueryFeedback")]
        [HttpGet]
        public object GetQueryFeedback(String classId, int unitId, int courseId)
        {
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("classId", classId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("courseId", courseId);
            var query2 = new BmobQuery();
            query2.WhereEqualTo("unitId", unitId);
            query1 = query1.And(query2);
            query = query.And(query1);
            //query.OrderByDescending("rightNum");
            var future = Bmob.FindTaskAsync<Feedback>("Feedback", query);
            try
            {
                feedbackList = future.Result.results;
                foreach (var f in feedbackList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Feedback_View feedback_view = new Feedback_View();
                    feedback_view.objectId = f.objectId;
                    feedback_view.username = f.username;
                    feedback_view.nickname = f.nickname;
                    feedback_view.totalNum = f.totalNum.Get();
                    feedback_view.rightNum = f.rightNum.Get();
                    if (feedback_view.totalNum != 0)
                    {
                        feedback_view.accuracy = 100 * feedback_view.rightNum / feedback_view.totalNum;
                    }
                    fList.Add(feedback_view);
                }
                List<Feedback_View> feedbackSortList = new List<Feedback_View>();
                feedbackSortList = fList.OrderByDescending(s => s.accuracy).ToList();
                return ResultToJson.toJson(feedbackSortList);
            }
            catch
            {
                return "获取失败";
            }
        }

        [Route("QueryFeedbackOfCourse")]
        [HttpGet]
        public object GetQueryFeedbackOfCourse(String classId, int courseId)
        {
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("classId", classId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("courseId", courseId);
            query = query.And(query1);
            //query.OrderByDescending("rightNum");
            var future = Bmob.FindTaskAsync<Feedback>("Feedback", query);
            try
            {
                feedbackList = future.Result.results;
                foreach (var f in feedbackList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Feedback_View feedback_view = new Feedback_View();
                    feedback_view.objectId = f.objectId;
                    feedback_view.username = f.username;
                    feedback_view.nickname = f.nickname;
                    feedback_view.totalNum = f.totalNum.Get();
                    feedback_view.rightNum = f.rightNum.Get();
                    if (feedback_view.totalNum != 0)
                    {
                        feedback_view.accuracy = 100 * feedback_view.rightNum / feedback_view.totalNum;
                    }
                    fList.Add(feedback_view);
                }
                List<Feedback_View> feedbackSortList = new List<Feedback_View>();
                feedbackSortList = fList.OrderByDescending(s => s.accuracy).ToList();
                return ResultToJson.toJson(feedbackSortList);
            }
            catch
            {
                return "获取失败";
            }
        }

        //学生答题数量统计
        [Route("QueryNum")]
        [HttpGet]
        public object GetQueryNum(String classId, int unitId, int courseId)
        {
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("classId", classId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("courseId", courseId);
            var query2 = new BmobQuery();
            query2.WhereEqualTo("unitId", unitId);
            query1 = query1.And(query2);
            query = query.And(query1);
            query.OrderByDescending("totalNum");
            var future = Bmob.FindTaskAsync<Feedback>("Feedback", query);
            try
            {
                feedbackList = future.Result.results;
                foreach (var f in feedbackList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Feedback_View feedback_view = new Feedback_View();
                    feedback_view.objectId = f.objectId;
                    feedback_view.username = f.username;
                    feedback_view.nickname = f.nickname;
                    feedback_view.totalNum = f.totalNum.Get();
                    fList.Add(feedback_view);
                }

                //SortedList s1 = new SortedList();
                //s1.Add(qList);
                return ResultToJson.toJson(fList);
            }
            catch
            {
                return "获取失败";
            }
        }

        [Route("QueryNumOfCourse")]
        [HttpGet]
        public object GetQueryNumOfCourse(String classId, int courseId)
        {
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("classId", classId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("courseId", courseId);
            query = query.And(query1);
            query.OrderByDescending("totalNum");
            var future = Bmob.FindTaskAsync<Feedback>("Feedback", query);
            try
            {
                feedbackList = future.Result.results;
                foreach (var f in feedbackList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Feedback_View feedback_view = new Feedback_View();
                    feedback_view.objectId = f.objectId;
                    feedback_view.username = f.username;
                    feedback_view.nickname = f.nickname;
                    feedback_view.totalNum = f.totalNum.Get();
                    fList.Add(feedback_view);
                }
                return ResultToJson.toJson(fList);
            }
            catch
            {
                return "获取失败";
            }
        }

    }
}