using cn.bmob.api;
using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TrainApp.WebApi.BmobModel;
using TrainApp.WebApi.ViewModel;

namespace TrainApp.WebApi.Controller
{
    
    [AllowAnonymous]
    public class ClassController : BmobBaseForm
    {

        public List<Class> classList = new List<Class>();

        [Route("ShowClass")]
        [HttpGet]
        public object GetClassInfo()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["UserInfoRemember"];
            String username = cookie["username"].ToString();
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String id = cookie1["CourseId"];
            int courseId = int.Parse(id);
            var query = new BmobQuery();
            query.WhereEqualTo("tId", username);
            var q1 = new BmobQuery();
            q1.WhereEqualTo("courseId", courseId);
            query.And(q1);
            var future = Bmob.FindTaskAsync<Class>("Class", query);
            try
            {
                classList = future.Result.results;
                return ResultToJson.toJson(classList);
            }
            catch
            {
                return "获取失败";
            }

        }

        [Route("AddClass")]
        [HttpPost]
        public object PostAddClass([FromBody]Class classInfo)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["UserInfoRemember"];
            String username = cookie["username"].ToString();
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String Id = cookie1["CourseId"];
            int courseId = int.Parse(Id);
            classInfo.tId = username;
            classInfo.courseId = courseId;
            var future = Bmob.CreateTaskAsync("Class", classInfo);
            String id = future.Result.objectId;
            return id;
        }

        [Route("DeleteClass")]
        [HttpGet]
        public object GetDeleteClass(String id)
        {
            var query = new BmobQuery();
            query.WhereEqualTo("id", id);
            var future = Bmob.FindTaskAsync<Class>("Class", query);
            try
            {
                String objectId = future.Result.results[0].objectId;
                var future1 = Bmob.DeleteTaskAsync("Class", objectId);
                return future1.Result.msg;
            }
            catch
            {
                return "获取失败";
            }

        }


        [Route("UpdateClass")]
        [HttpPost]
        public object PostUpdateClass([FromBody]List<Class_View> classInfo)
        {
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String Id = cookie1["CourseId"];
            int courseId = int.Parse(Id);
            String result = "";
            int count = 0;
            int length = classInfo.Count;
            BmobQuery query = new BmobQuery();
            
            for (int i = 0; i < length; i++)
            {
                query.WhereEqualTo("id", classInfo[i].id);
                var future = Bmob.FindTaskAsync<Class>("Class", query);
                try
                {
                    String objectId = future.Result.results[0].objectId;
                    Class c = new Class();
                    c.id = classInfo[i].id;
                    c.name = classInfo[i].name;
                    c.courseId = BmobInput.Parse<BmobInt>(courseId);
                    c.schedule = classInfo[i].schedule;
                    c.term = classInfo[i].term;
                    var future1 = Bmob.UpdateTaskAsync("Class", objectId, c);
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
                }catch
                {
                    result = "fail";
                }
            }
            //for(int i = 0; i < length; i++)
            //{
            //    String objectId = classInfo[i].objectId;
            //    var future = Bmob.UpdateTaskAsync("Class", objectId, classInfo[i]);
            //    try
            //    {
            //        String a = future.Result.updatedAt;
            //        count++;
            //        if(count == length)
            //        {
            //            result = "success";
            //        }
            //    }
            //    catch
            //    {
            //        result = "fail";
            //    }
            //}
            return result;

        }


    }
}