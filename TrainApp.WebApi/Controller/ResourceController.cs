using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TrainApp.WebApi.BmobModel;
using TrainApp.WebApi.ViewModel;

namespace TrainApp.WebApi.Controller
{
    [AllowAnonymous]
    public class ResourceController : BmobBaseForm
    {

        public List<Resource> resourceList = new List<Resource>();

        [Route("ShowResource")]
        [HttpGet]
        public object GetResourceInfo(int courseId, int unitId, int knowledgeId) {
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", courseId);
            if(unitId != 0)
            {
                var q1 = new BmobQuery();
                q1.WhereEqualTo("unitId", unitId);
                query.And(q1);
            }
            if(knowledgeId != 0)
            {
                var q1 = new BmobQuery();
                q1.WhereEqualTo("knowledgeId", knowledgeId);
                query.And(q1);
            }
            var future = Bmob.FindTaskAsync<Resource>("Resource", query);
            try
            {
                resourceList = future.Result.results;
                List<Resource_View> resourceView = new List<Resource_View>();
                foreach(var resource in resourceList)
                {
                    Resource_View resources = new Resource_View();
                    resources.id = resource.id.Get();
                    resources.fileUrl = resource.file.url;
                    resources.fileName = resource.file.filename;
                    resources.type = resource.type;
                    resourceView.Add(resources);
                }
                return ResultToJson.toJson(resourceView);
            }
            catch
            {
                return "fail";
            }
        }

        [Route("DeleteResource")]
        [HttpGet]
        public object GetDeleteResource(int id)
        {
            var query = new BmobQuery();
            query.WhereEqualTo("id", id);
            var future = Bmob.FindTaskAsync<Resource>("Resource", query);
            try
            {
                String objectId = future.Result.results[0].objectId;
                var future1 = Bmob.DeleteTaskAsync("Resource", objectId);
                return future1.Result.msg;
            }
            catch
            {
                return "获取失败";
            }

        }

        [Route("uploadResource")]
        [HttpPost]
        public object PostFile([FromBody]Resource_View resourceView)
        {
            String a = "";
           // String objectId = "";
            Resource resource = new Resource();
            resource.id = BmobInput.Parse<BmobInt>(resourceView.id);
            resource.file = resourceView.file;
            resource.type = resourceView.type;
            resource.courseId = BmobInput.Parse<BmobInt>(resourceView.courseId);
            resource.unitId = BmobInput.Parse<BmobInt>(resourceView.unitId);
            resource.knowledgeId = BmobInput.Parse<BmobInt>(resourceView.knowledgeId);
            //var query = new BmobQuery();
            //query.WhereEqualTo("id", course.id);
            var f = Bmob.CreateTaskAsync("Resource", resource);
            try
            {

                a = f.Result.objectId;
            }
            catch
            {
                a = "失败";
            }
            return a;
        }
    }
}