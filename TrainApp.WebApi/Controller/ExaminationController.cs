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
            List<Question_View> resultList = new List<Question_View>();
            resultList = randomPaper(requirement);
            return ResultToJson.toJson(resultList);
        }

        //随机组卷
        public List<Question_View> randomPaper(Requirement require)
        {
            //数据初始化
            int n = 5;//难度等级数 1.0,1.5,2.0,2.5,3.0
            int k = n + 1;
            int realNum = 0;
            Double paperDifficulty = Convert.ToDouble(require.difficulty);//试卷难度系数
            //String[] weight = require.unitWeight.Split(new[] { ';' });
            //Double[] unitWeight = new Double[weight.Length];
            //for (int i = 0; i < weight.Length; i++)
            //{
            //    unitWeight[i] = Convert.ToDouble(weight[i]);
            //}
            //计算每个难度级别的题目个数
            Double avg = paperDifficulty / (Double)(k + 1); //平均期望
            Double pa = 1;//组合数的值
            Double[] p = new Double[k + 1];
            //计算每种难度级别抽取百分比
            p[0] = pa * Math.Pow((1 - avg), k);
            for (int i = 1; i <= k; i++)
            {
                pa = pa * ((k - i + 1) / i);
                p[i] = pa * Math.Pow(avg, i) * Math.Pow((1 - avg), k - i);
            }
            p[1] = p[1] + p[0];
            p[k - 1] = p[k] + p[k - 1];
            //计算每种难度级别抽取的题目个数
            int[] q = new int[k];
            for (int i = 1; i < k; i++)
            {
                q[i] = (int)(p[i] * require.totalNum);
                realNum = realNum + q[i];
            }
            //补充缺少的题目
            if(realNum != require.totalNum)
            {
                int m = require.totalNum - realNum;
                int temp = 0;
                Double min = 3;
                for(int i = 1; i < k; i++)
                {
                    if(min > Math.Abs(0.5 + 0.5 * i - paperDifficulty))
                    {
                        min = Math.Abs(0.5 + 0.5 * i - paperDifficulty);
                        temp = i;
                    }

                }
                q[temp] = q[temp] + m;

            }
            //开始抽取题目（随机数）
            List<Question_View> resultList = new List<Question_View>();
            for(int i = 1; i <= 5; i++)
            {
                Double diff = 0.5 + 0.5 * i;
                resultList.AddRange(selectQuestion(require, diff, q[i]));
            }
            return resultList;
        }

        public List<Question_View> selectQuestion(Requirement require, Double diff, int num)
        {
            List<Question> questionList = new List<Question>();
            List<Question_View> qList = new List<Question_View>();
            //List<Question_View> resultList = new List<Question_View>();
            var query = new BmobQuery();
            query.WhereEqualTo("courseId", require.courseId);
            var q1 = new BmobQuery();
            q1.WhereEqualTo("difficulty", diff);
            query.And(q1);
            query.Limit(500);
            var future = Bmob.FindTaskAsync<Question>("Question", query);
            try
            {
                questionList = future.Result.results;
                Random ra = new Random();
                int[] qNum = new int[num];
                int i = 0;
                while (true)
                {
                    if(i == num-1)
                    {
                        break;
                    }
                    int random = ra.Next(0,questionList.Count);
                    int n = 0;
                    for (int k = 0; k < i; k++)
                    {
                        if (qNum[k] != random)
                        {
                            n++;
                        }
                    }
                    if (n == i)
                    {
                        qNum[i] = random;
                    }
                    else
                    {
                        continue;
                    }
                    i++;
                }
                for(int a = 0; a < num; a++)
                {
                    int id = qNum[a];
                    Question q = new Question();
                    q = questionList[id];
                    Question_View question_view = new Question_View();
                    question_view.id = q.id.Get();
                    question_view.question = q.question;
                    question_view.a = q.a;
                    question_view.b = q.b;
                    question_view.c = q.c;
                    question_view.d = q.d;
                    question_view.answer = q.answer;
                    question_view.analysis = q.analysis;
                    question_view.difficulty = q.difficulty.Get();
                    question_view.unitId = q.unitId.Get();
                    question_view.knowledgeId = q.knowledgeId;
                    qList.Add(question_view);
                }
            }
            catch
            {
                //result = "抽取失败";
            }
            return qList;
        }


    }
}