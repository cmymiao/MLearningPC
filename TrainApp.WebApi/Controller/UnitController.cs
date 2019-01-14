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
    [Route("QueryUnit")]                 //定义访问该控制器中的路由（地址）名称
    [AllowAnonymous]                    //定义使用匿名访问方式访问路由地址
    public class UnitController : BmobBaseForm          //需继承BmobBaseForm
    {
        public Unit unit = new Unit();
        public List<Unit> unitList = new List<Unit>();
        public List<Unit_View> uList = new List<Unit_View>();


        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object Get(int courseId)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("courseId", courseId);
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

        [Route("SelectedUnit")]
        [HttpGet]
        public object GetSelectedUnit(int courseId)
        {
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
    }
}