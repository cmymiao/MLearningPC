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
    [AllowAnonymous]                    //定义使用匿名访问方式访问路由地址
    public class ExaminationController : BmobBaseForm          //需继承BmobBaseForm
    {
        public Examination examination = new Examination();
        public List<Examination> examinationList = new List<Examination>();
        public List<Examination_View> eList = new List<Examination_View>();

        [Route("QueryExamination")]
        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object Get(int courseId)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("courseId", courseId);
            query.OrderBy("id");
            var future = Bmob.FindTaskAsync<Examination>("Examination", query);
            try
            {
                examinationList = future.Result.results;
                foreach (var e in examinationList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Examination_View examination_View = new Examination_View();
                    examination_View.objectId = e.objectId;
                    examination_View.name = e.name;
                    examination_View.questionList = e.questionList;
                    examination_View.id = e.id.Get();
                    examination_View.difficulty = e.difficulty.Get();
                    examination_View.courseId = e.courseId.Get();
                    eList.Add(examination_View);
                }
            }
            catch
            {

            }
            return ResultToJson.toJson(eList);      //返回数据需要json格式

        }

        [Route("DeleteExamination")]
        [HttpGet]
        public object GetDeleteExamination(int id)
        {
            var query = new BmobQuery();
            query.WhereEqualTo("id", id);
            var future = Bmob.FindTaskAsync<Examination>("Examination", query);
            try
            {
                String objectId = future.Result.results[0].objectId;
                var future1 = Bmob.DeleteTaskAsync("Examination", objectId);
                return future1.Result.msg;
            }
            catch
            {
                return "获取失败";
            }

        }

        [Route("ExaminationDetails")]
        [HttpGet]
        public object GetExaminationDetails(int id)
        {
            var query = new BmobQuery();
            query.WhereEqualTo("id", id);
            var future = Bmob.FindTaskAsync<Examination>("Examination", query);
            examinationList = future.Result.results;
            try
            {
                examinationList = future.Result.results;
                foreach (var e in examinationList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Examination_View examination_View = new Examination_View();
                    examination_View.objectId = e.objectId;
                    examination_View.name = e.name;
                    examination_View.questionList = e.questionList;
                    examination_View.id = e.id.Get();
                    examination_View.difficulty = e.difficulty.Get();
                    examination_View.courseId = e.courseId.Get();
                    if(examination_View.questionList != "")
                    {
                        string[] results = examination_View.questionList.Split(new[] { ';' });
                        examination_View.examQuestion = Array.ConvertAll<string, int>(results, s => int.Parse(s));
                    }
                    eList.Add(examination_View);
                }
            }
            catch
            {

            }
            return ResultToJson.toJson(eList);      //返回数据需要json格式
        }

       
    }
}