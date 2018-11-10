using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TrainApp.WebApi.BmobModel;

namespace TrainApp.WebApi
{

    [AllowAnonymous]
    public class UserController : BmobBaseForm
    {

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
    }

}




