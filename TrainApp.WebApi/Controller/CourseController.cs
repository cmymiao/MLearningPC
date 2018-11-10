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
        

        [HttpGet]                       //定义访问方式（Post或Get方法）
       public object Get()             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            var query = new BmobQuery();
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
                    course_view.times = "无";
                    course_view.timeu = "";
                }
                if (c.program != null) {
                    course_view.programs = "查看课程大纲";
                    course_view.programu = c.program.url;
                }
                else {
                    course_view.programs = "无";
                    course_view.programu = "../inview/class1.html";
                }
                if (c.experiment != null) {
                    course_view.experiments = "查看实验大纲";
                    course_view.experimentu = c.experiment.url;
                }
                else {
                    course_view.experiments = "无";
                    course_view.experimentu = "../inview/class1.html";
                }
                cList.Add(course_view);
                
            }
           return ResultToJson.toJson(cList);      //返回数据需要json格式

        }



        [Route("Select")]
        [HttpGet]
        public object Getselect(int id)
        {
            var query = new BmobQuery();
            query.WhereEqualTo("id", id);
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
                        course_view.times = "上传教学日历";
                        course_view.timeu = "../inview/upload3.html";
                    }
                    if (c.program != null)
                    {
                        course_view.programs = "查看课程大纲";
                        course_view.programu = c.program.url;
                    }
                    else
                    {
                        course_view.programs = "上传课程大纲";
                        course_view.programu = "../inview/upload1.html";
                    }
                    if (c.experiment != null)
                    {
                        course_view.experiments = "查看实验大纲";
                        course_view.experimentu = c.experiment.url;
                    }
                    else
                    {
                        course_view.experiments = "上传实验大纲";
                        course_view.experimentu = "../inview/upload2.html";
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


   


        [Route("uploadfile")]
        [HttpPost]
        public object PostFile([FromBody]Course_View cl)
        {
            String a = "";
            String objectId = "";
            Course course = new Course();
            course.id = BmobInput.Parse<BmobInt>(cl.id);
            course.program = cl.program;
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

        [Route("uploadfilee")]
        [HttpPost]
        public object PostFilee([FromBody]Course_View cl)
        {
            String a = "";
            String objectId = "";
            Course course = new Course();
            course.id = BmobInput.Parse<BmobInt>(cl.id);
            course.experiment = cl.experiment;
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

        [Route("uploadfilet")]
        [HttpPost]
        public object PostFilet([FromBody]Course_View cl)
        {
            String a = "";
            String objectId = "";
            Course course = new Course();
            course.id = BmobInput.Parse<BmobInt>(cl.id);
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