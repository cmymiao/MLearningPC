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
    [Route("QueryKnowledge")]                 //定义访问该控制器中的路由（地址）名称
    [AllowAnonymous]                    //定义使用匿名访问方式访问路由地址
    public class KnowledgeController : BmobBaseForm          //需继承BmobBaseForm
    {
        public Knowledge knowledge = new Knowledge();
        public List<Knowledge> knowledgeList = new List<Knowledge>();
        public List<Knowledge_View> kList = new List<Knowledge_View>();


        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object Get(int unitId)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            var query = new BmobQuery();
            query.Limit(300);
            query.WhereEqualTo("unitId", unitId);
            var future = Bmob.FindTaskAsync<Knowledge>("Knowledge", query);
            knowledgeList = future.Result.results;
            foreach (var k in knowledgeList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
            {
                Knowledge_View knowledge_view = new Knowledge_View();
                knowledge_view.id = k.id.Get();
                knowledge_view.name = k.name;
                knowledge_view.courseId = k.courseId.Get();
                knowledge_view.unitId = k.unitId.Get();

                kList.Add(knowledge_view);

            }
            return ResultToJson.toJson(kList);      //返回数据需要json格式

        }
        [Route("SelectedKnowledge")]
        [HttpGet]
        public object GetSelectedKnowledge(int unitId,int courseId)
        {
            String objectId = "";
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", courseId);
            var f = Bmob.FindTaskAsync<Knowledge>("Knowledge", query);
            try
            {
                query.WhereEqualTo("unitId", unitId);
                var future = Bmob.FindTaskAsync<Knowledge>("Knowledge", query);
                knowledgeList = future.Result.results;
                foreach (var k in knowledgeList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    objectId = f.Result.results[0].objectId;
                    Knowledge_View knowledge_view = new Knowledge_View();
                    knowledge_view.id = k.id.Get();
                    knowledge_view.name = k.name;
                    knowledge_view.courseId = k.courseId.Get();
                    knowledge_view.unitId = k.unitId.Get();
                    kList.Add(knowledge_view);
                }
                return ResultToJson.toJson(kList);
            }
            catch
            {
                return "获取失败";
            }
        }

        [Route("ShowAllKnowledge")]
        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object GetKnowledge(int courseId)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", courseId);
            query.OrderBy("id");
            var future = Bmob.FindTaskAsync<Knowledge>("Knowledge", query);
            try
            {
                knowledgeList = future.Result.results;
                foreach (var k in knowledgeList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Knowledge_View knowledge_view = new Knowledge_View();
                    knowledge_view.id = k.id.Get();
                    knowledge_view.name = k.name;
                    knowledge_view.courseId = k.courseId.Get();
                    knowledge_view.unitId = k.unitId.Get();

                    kList.Add(knowledge_view);

                }
            }catch
            {

            }
            return ResultToJson.toJson(kList);      //返回数据需要json格式

        }

        [Route("DeleteKnowledge")]
        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object GetDeleteKnowledge(int id, int courseId)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            String result = "";
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", courseId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("id", id);
            query.And(query1);
            var future = Bmob.FindTaskAsync<Knowledge>("Knowledge", query);
            try
            {
                String objectId = future.Result.results[0].objectId;
                var future1 = Bmob.DeleteTaskAsync("Knowledge", objectId);
                try
                {
                    result = future1.Result.msg;
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

        [Route("UpdateKnowledge")]
        [HttpGet]                       //定义访问方式（Post或Get方法）
        public object GetUpdateKnowledge(int id, int courseId, String name)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            String result = "";
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", courseId);
            var query1 = new BmobQuery();
            query1.WhereEqualTo("id", id);
            query.And(query1);
            var future = Bmob.FindTaskAsync<Knowledge>("Knowledge", query);
            try
            {
                String objectId = future.Result.results[0].objectId;
                Knowledge k = new BmobModel.Knowledge();
                k = future.Result.results[0];
                k.name = name;
                var future1 = Bmob.UpdateTaskAsync("Knowledge", objectId, k);
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

        [Route("AddKnowledge")]
        [HttpPost]                       //定义访问方式（Post或Get方法）
        public object PostAddKnowledge([FromBody]Knowledge_View knowledge_view)             //返回数据的函数，如果是Get方法则方法名为Get开头，若是Post则使用Post开头。
        {
            String result = "";
            Knowledge knowledge = new Knowledge();
            knowledge.id = knowledge_view.id;
            knowledge.name = knowledge_view.name;
            knowledge.courseId = knowledge_view.courseId;
            knowledge.unitId = knowledge_view.unitId;
            var future = Bmob.CreateTaskAsync("Knowledge", knowledge);
            try
            {
                result = future.Result.objectId;
            }
            catch
            {
                result = "失败";
            }
            return result;
        }

    }


}