using cn.bmob.api;
using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TrainApp.WebApi.BmobModel;

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
            var query = new BmobQuery();
            query.WhereEqualTo("tId", username);
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
            classInfo.tId = username;
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
        public object PostUpdateClass([FromBody]List<Class> classInfo)
        {
            String result = "";
            int count = 0;
            int length = classInfo.Count;
            for(int i = 0; i < length; i++)
            {
                String objectId = classInfo[i].objectId;
                var future = Bmob.UpdateTaskAsync("Class", objectId, classInfo[i]);
                try
                {
                    String a = future.Result.updatedAt;
                    count++;
                    if(count == length)
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


    }
}