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

    }
}