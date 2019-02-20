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
                   //定义访问该控制器中的路由（地址）名称
    [AllowAnonymous]                    //定义使用匿名访问方式访问路由地址
    public class UnitController : BmobBaseForm          //需继承BmobBaseForm
    {
        public Unit unit = new Unit();
        public List<Unit> unitList = new List<Unit>();
        public List<Unit_View> uList = new List<Unit_View>();

        [Route("QueryUnit")]
        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object Get()             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String id = cookie1["CourseId"];
            int courseId = int.Parse(id);
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("courseId", courseId);
            query.OrderBy("id");
            var future = Bmob.FindTaskAsync<Unit>("Unit", query);
            unitList = future.Result.results;
            foreach (var u in unitList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
            {
                Unit_View unit_view = new Unit_View();
                unit_view.id = u.id.Get();
                unit_view.name = u.name;
                unit_view.courseId = u.courseId.Get();

                uList.Add(unit_view);

            }
            return ResultToJson.toJson(uList);      //返回数据需要json格式

        }

        //[Route("ShowUnit")]
        //[HttpGet]
        //public object GetShowUnit()
        //{
        //    var query = new BmobQuery();
        //    var future = Bmob.FindTaskAsync<Unit>("Unit", query);
        //    unitList = future.Result.results;
        //    foreach (var u in unitList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
        //    {
        //        Unit_View unit_view = new Unit_View();
        //        unit_view.id = u.id.Get();
        //        unit_view.name = u.name;
        //        uList.Add(unit_view);

        //    }
        //    return ResultToJson.toJson(uList);

        //}

        [Route("SelectUnit")]
        [HttpGet]
        public object GetSelectUnit(int id)
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

        //试卷题目修改模态框
        [Route("SelectedUnit")]
        [HttpGet]
        public object GetSelectedUnit()
        {
            HttpCookie cookie1 = HttpContext.Current.Request.Cookies["CurrentCourse"];
            String id = cookie1["CourseId"];
            int courseId = int.Parse(id);
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", courseId);
            var future = Bmob.FindTaskAsync<Unit>("Unit", query);
            try
            {
                unitList = future.Result.results;
                foreach (var u in unitList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Unit_View unit_view = new Unit_View();
                    unit_view.id = u.id.Get();
                    unit_view.name = u.name;
                    unit_view.courseId = u.courseId.Get();
                    uList.Add(unit_view);
                }
                return ResultToJson.toJson(uList);
            }
            catch
            {
                return "获取失败";
            }
        }

        [Route("DeleteUnit")]
        [HttpGet]
        public object GetDeleteUnit(int courseId, int id)
        {
            String result = "";
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", courseId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("id", id);
            query1.And(query);
            var future = Bmob.FindTaskAsync<Unit>("Unit", query1);
            try
            {
                String objectId = future.Result.results[0].objectId;
                var future1 = Bmob.DeleteTaskAsync("Unit", objectId);
                try
                {
                    var query2 = new BmobQuery();
                    query2.WhereEqualTo("unitId", id);
                    query2.And(query);
                    var future2 = Bmob.FindTaskAsync<Knowledge>("Knowledge", query2);
                    try
                    {
                        int length = future2.Result.results.Count;
                        for (int i = 0; i < length; i++)
                        {
                            String objectid = future2.Result.results[i].objectId;
                            var future3 = Bmob.DeleteTaskAsync("Knowledge", objectid);
                            try
                            {
                                result = future1.Result.msg;
                            }
                            catch
                            {
                                result = "删除失败";
                            }
                        }
                    }catch
                    {
                        result = "删除失败";
                    }
                }catch
                {
                    result = "删除失败";
                }
            }
            catch
            {
                result = "删除失败";
            }
            return result;
        }

        [Route("UpdateUnit")]
        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object GetUpdateUnit(int id, int courseId, String name)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            String result = "";
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", courseId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("id", id);
            query.And(query1);
            var future = Bmob.FindTaskAsync<Unit>("Unit", query);
            try
            {
                String objectId = future.Result.results[0].objectId;
                Unit k = new BmobModel.Unit();
                k = future.Result.results[0];
                k.name = name;
                var future1 = Bmob.UpdateTaskAsync("Unit", objectId, k);
                try
                {
                    result = future1.Result.updatedAt;
                }
                catch
                {
                    result = "修改失败";
                }
            }
            catch
            {
                result = "修改失败";
            }
            return result;
        }

        [Route("AddUnit")]
        [HttpPost]                       //定义访问方式（Post或Get方法）
        public object PostAddUnit([FromBody]Unit_View unit_view)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            String result = "";
            Unit unit = new Unit();
            unit.id = unit_view.id;
            unit.name = unit_view.name;
            unit.courseId = unit_view.courseId;
            var future = Bmob.CreateTaskAsync("Unit", unit);
            try
            {
                result = future.Result.objectId;
            }catch
            {
                result = "失败";
            }
            return result;
        }
    }
}