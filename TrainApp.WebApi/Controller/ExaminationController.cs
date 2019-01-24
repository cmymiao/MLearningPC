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

        [Route("RandomPaper")]
        [HttpPost]
        public object PostRandomPaper([FromBody]Requirement requirement)
        {
            //String result = "";
            List<Question> questionList = new List<Question>();
            List<Question_View> qList = new List<Question_View>();
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", requirement.courseId);
            query.Limit(500);
            var future = Bmob.FindTaskAsync<Question>("Question", query);
            try
            {
                questionList = future.Result.results;
                foreach (var q in questionList)     //由于BmobModel中有BmobInt类型不能直接显示到页面中，所以需要对字段的类型进行处理，变为相对应的ViewModel格式。
                {
                    Question_View question_view = new Question_View();
                    question_view.id = q.id.Get();
                    question_view.difficulty = q.difficulty.Get();
                    question_view.unitId = q.unitId.Get();
                    question_view.knowledgeId = q.knowledgeId;
                    qList.Add(question_view);
                }
                randomPaper(qList, requirement);
            }
            catch
            {
                //result = "组卷失败";
            }
            return "";
        }

        //随机组卷
        public object randomPaper(List<Question_View> questionList, Requirement require)
        {
            //数据初始化
            int n = 5;
            Double paperDifficulty = Convert.ToDouble(require.difficulty);//试卷难度系数
            String[] weight = require.unitWeight.Split(new[] { ';' });
            Double[] unitWeight = new Double[weight.Length];
            for(int i = 0; i < weight.Length; i++)
            {
                unitWeight[i] = Convert.ToDouble(weight[i]);
            }
            //计算每个难度级别的题目个数
            Double p = paperDifficulty / (Double)(n + 2); //平均期望


            return "";
        }

       
    }
}