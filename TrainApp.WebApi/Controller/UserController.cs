using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using TrainApp.WebApi.BmobModel;

namespace TrainApp.WebApi
{

    [AllowAnonymous]
    public class UserController : BmobBaseForm
    {
        private String applicationID = "f69acbf2dd96fbaefdf9fd9793e93f66";
        private String restKey = "9a956445ff932b7d3f59b81af28cbe2a";
        private String masterKey = "9da6ffb13f5a0d6d26de8674d169bbdf";
        private String url = "https://api2.bmob.cn/1/users/";

        [Route("UserLogin")]
        [HttpGet]
        public object GetUserLogin(String username, String password)
        {
            String objectId = login(username, password);
            return objectId;
        }

        public String login(String username, String password)
        {
            String id = "";
            User user = new User(); 
            user.username = username;
            user.password = password;
            var future = Bmob.LoginTaskAsync<User>(username, password);
            try
            {
                if (future.Result.identity.Equals("teacher"))
                {
                    id = "登录成功" + future.Result.objectId;
                    HttpCookie cookie = new HttpCookie("UserInfoRemember");
                    cookie["UserName"] = username;
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    id = "登录失败";
                } 
            }
            catch
            {
                id = "登录失败";
            }
            return id;
        }

        [Route("ResetPassword")]
        [HttpGet]
        public object GetReset(String email)
        {
            String result = "";
            var future = Bmob.ResetTaskAsync(email);
            try
            {
                result = future.Result.ToString();
            }
            catch
            {
                result = future.Result.ToString();
            }
            return result;
        }

        List<User> userList = new List<User>();

        [Route("UploadStudent")]
        [HttpPost]
        public object PostAddStudent([FromBody]List<User> user1)
        {
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String Id = cookie1["CourseId"];
            int courseId = int.Parse(Id);
            String a = "";
            List<User> error = new List<User>();
            List<String> list = new List<String>();
            userList.AddRange(user1);
            int length = userList.Count;
            for (int i = 0; i < userList.Count; i++)
            {
                User user = new User();
                user.username = userList[i].username;
                user.password = userList[i].password;
                user.name = userList[i].name;
                user.identity = userList[i].identity;
                user.classId = userList[i].classId;
                user.firstTime = 0;
                user.courseId = courseId;

                var future = Bmob.SignupTaskAsync(user);
                try
                {
                    String id = future.Result.objectId;
                    list.Add(id);
                }
                catch
                {
                    error.Add(user);
                }

            }
            if (userList.Count == list.Count)
            {
                a = "上传成功";
                return a;
            }
            else
            {
                return ResultToJson.toJson(error);
            }
            
        }

        [Route("showAllStudent")]
        [HttpGet]
        public object GetAllStudent()
        {
            var query = new BmobQuery();
            //query.Skip((pageIndex - 1) * pageSize).Limit(pageSize);
            HttpCookie cookie = HttpContext.Current.Request.Cookies["UserInfoRemember"];
            String username = cookie["username"].ToString();
            query.WhereEqualTo("tId", username);
            var future1 = Bmob.FindTaskAsync<Class>("Class", query);
            try
            {
                int count = future1.Result.results.Count;
                String[] classes = new String[count];
                for(int i = 0; i < count; i++)
                {
                    classes[i] = future1.Result.results[i].id;
                }
                var query1 = new BmobQuery();
                query1.WhereContainedIn("classId", classes);
                var future = Bmob.FindTaskAsync<User>("_User", query1);
                try
                {
                    userList = future.Result.results;
                    return ResultToJson.toJson(userList);
                }
                catch
                {
                    return "获取失败";
                }
            }
            catch
            {
                return "获取失败";
            }
            
        }

        [Route("showStudentById")]
        [HttpGet]
        public object GetStudentById(String classId)
        {
            var query = new BmobQuery();
            query.WhereEqualTo("classId", classId);
            var future = Bmob.FindTaskAsync<User>("_User", query);
            try
            {
                userList = future.Result.results;
                return ResultToJson.toJson(userList);
            }
            catch
            {
                return "获取失败";
            }

        }

        [Route("AddStudent")]
        [HttpPost]
        public object PostAddStudent([FromBody]User userInfo)
        {
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String Id = cookie1["CourseId"];
            int courseId = int.Parse(Id);
            String id = "";
            userInfo.identity = "student";
            userInfo.firstTime = 0;
            userInfo.courseId = courseId;
            var future = Bmob.SignupTaskAsync(userInfo);
            try
            {
                id = "success";
            }
            catch
            {
                id = "fail";
            }
            
            return id;
        }

        [Route("DeleteStudent")]
        [HttpGet]
        public object GetDeleteStudent(String objectId)
        {
            String Url = url + objectId;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "DELETE";
            request.ContentType = "application/json";
            request.Headers.Add("X-Bmob-Application-Id", applicationID);
            request.Headers.Add("X-Bmob-REST-API-Key", restKey);
            request.Headers.Add("X-Bmob-Master-Key", masterKey);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.StatusCode;
        }

    }
}




