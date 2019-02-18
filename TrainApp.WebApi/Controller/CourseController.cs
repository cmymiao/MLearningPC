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
    [Route("ShowCourse")]                 //定义访问该控制器中的路由（地址）名称
    [AllowAnonymous]                    //定义使用匿名访问方式访问路由地址
    public class CourseController : BmobBaseForm          //需继承BmobBaseForm
    {
        public Course course = new Course();
        public List<Course> courseList = new List<Course>();
        public List<Course_View> cList = new List<Course_View>();
        public List<Course_View> cl = new List<Course_View>();
        //public List<Course_View> cl2 = new List<Course_View>();

        [Route("SaveCurrentCourse")]
        [HttpGet]
        public object GetCurrentCourse(String id)
        {
            HttpCookie cookie = new HttpCookie("CurrentCourse");
            cookie["CourseId"] = id;
            HttpContext.Current.Response.Cookies.Add(cookie);
            return "Success";
        }
        

        [HttpGet]                       //定义访问方式（Post或Get方法）
       public object Get()             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            var query = new BmobQuery();
            HttpCookie cookie = HttpContext.Current.Request.Cookies["UserInfoRemember"];
            String username = cookie["username"].ToString();
            query.WhereEqualTo("tId", username);
            query.Limit(300);
            var future = Bmob.FindTaskAsync<Course>("Course", query);
            courseList = future.Result.results;
            foreach (var c in courseList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
            {
                Course_View course_view = new Course_View();
                course_view.id = c.id.Get();
                course_view.name = c.name;
                if (c.time != null) {
                    course_view.times = "查看教学日历";
                    course_view.timeu = c.time.url;          
                }
                else { 
                    course_view.times = "暂无教学日历";
                    course_view.timeu = "";
                }
                if (c.program != null) {
                    course_view.programs = "查看课程大纲";
                    course_view.programu = c.program.url;
                }
                else {
                    course_view.programs = "暂无课程大纲";
                    course_view.programu = "";
                }
                if (c.experiment != null) {
                    course_view.experiments = "查看实验大纲";
                    course_view.experimentu = c.experiment.url;
                }
                else {
                    course_view.experiments = "暂无实验大纲";
                    course_view.experimentu = "";
                }
                cList.Add(course_view);
                
            }
           return ResultToJson.toJson(cList);      //返回数据需要json格式

        }


        [Route("ShowCourseById")]
        [HttpGet]
        public object Getselect()
        {
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String id = cookie1["CourseId"];
            int courseId = int.Parse(id);
            var query = new BmobQuery();
            query.WhereEqualTo("id", courseId);
            var future = Bmob.FindTaskAsync<Course>("Course", query);
            try
            {
                courseList = future.Result.results;
                foreach (var c in courseList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Course_View course_view = new Course_View();
                    course_view.id = c.id.Get();
                    course_view.name = c.name;
                    if (c.time != null)
                    {
                        course_view.times = "查看教学日历";
                        course_view.timeu = c.time.url;
                    }
                    else
                    {
                        course_view.times = "暂无教学日历";
                        course_view.timeu = "";
                    }
                    if (c.program != null)
                    {
                        course_view.programs = "查看课程大纲";
                        course_view.programu = c.program.url;
                    }
                    else
                    {
                        course_view.programs = "暂无课程大纲";
                        course_view.programu = "";
                    }
                    if (c.experiment != null)
                    {
                        course_view.experiments = "查看实验大纲";
                        course_view.experimentu = c.experiment.url;
                    }
                    else
                    {
                        course_view.experiments = "暂无实验大纲";
                        course_view.experimentu = "";
                    }
                    cList.Add(course_view);
                }
                return ResultToJson.toJson(cList);
            }
            catch
            {
                return "获取失败";
            }

        }

        [Route("addCourse")]
        [HttpPost]
        public object PostAddCourse([FromBody]Course_View cl)
        {
            String a = "";
            HttpCookie cookie = HttpContext.Current.Request.Cookies["UserInfoRemember"];
            String username = cookie["username"].ToString();
            Course course = new Course();
            course.id = BmobInput.Parse<BmobInt>(cl.id);
            course.name = cl.name;
            course.tId = username;
            //course.program = cl.program;
            //course.experiment = cl.experiment;
            //course.time = cl.time;
            var future = Bmob.CreateTaskAsync("Course", course);
            try
            {
                a = "success";
            }catch
            {
                a = "fail";
            }
            return a;
        }

        [Route("uploadFile")]
        [HttpPost]
        public object PostFile([FromBody]Course_View cl)
        {
            String a = "";
            String objectId = "";
            Course course = new Course();
            course.id = BmobInput.Parse<BmobInt>(cl.id);
            course.name = cl.name;
            course.program = cl.program;
            course.experiment = cl.experiment;
            course.time = cl.time;
            var query = new BmobQuery();
            query.WhereEqualTo("id", course.id);
            var f = Bmob.FindTaskAsync<Course>("Course", query);
            try
            {
              
                objectId = f.Result.results[0].objectId;
                var future = Bmob.UpdateTaskAsync("Course", objectId, course);
                a = future.Result.updatedAt;
            }
            catch
            {
                a = "失败";
            }
            return a;
        }

        //[Route("uploadfilee")]
        //[HttpPost]
        //public object PostFilee([FromBody]Course_View cl)
        //{
        //    String a = "";
        //    String objectId = "";
        //    Course course = new Course();
        //    course.id = BmobInput.Parse<BmobInt>(cl.id);
        //    course.experiment = cl.experiment;
        //    var query = new BmobQuery();
        //    query.WhereEqualTo("id", course.id);
        //    var f = Bmob.FindTaskAsync<Course>("Course", query);
        //    try
        //    {

        //        objectId = f.Result.results[0].objectId;
        //        var future = Bmob.UpdateTaskAsync("Course", objectId, course);
        //        a = future.Result.updatedAt;
        //    }
        //    catch
        //    {
        //        a = "失败";
        //    }
        //    return a;
        //}

        //[Route("uploadfilet")]
        //[HttpPost]
        //public object PostFilet([FromBody]Course_View cl)
        //{
        //    String a = "";
        //    String objectId = "";
        //    Course course = new Course();
        //    course.id = BmobInput.Parse<BmobInt>(cl.id);
        //    course.time = cl.time;
        //    var query = new BmobQuery();
        //    query.WhereEqualTo("id", course.id);
        //    var f = Bmob.FindTaskAsync<Course>("Course", query);
        //    try
        //    {

        //        objectId = f.Result.results[0].objectId;
        //        var future = Bmob.UpdateTaskAsync("Course", objectId, course);
        //        a = future.Result.updatedAt;
        //    }
        //    catch
        //    {
        //        a = "失败";
        //    }
        //    return a;
        //}

        [Route("SelectCourse")]
        [HttpGet]
        public object GetSelectCourse(int id)
        {
            //var query = new BmobQuery();
            //query.WhereEqualTo("id", id);
            //var future = Bmob.FindTaskAsync<Course>("Course", query);
            try
            {
                return id;
            }
            catch
            {
                return "获取失败";
            }

        }


    }
}