using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using cn.bmob.api;

using cn.bmob.tools;
using System.Diagnostics;
using System.Web.Http;

namespace TrainApp.WebApi
{
    public class BmobBaseForm : ApiController       //初始化Bmob，不用修改
    {
        private BmobWindows bmob;
        public BmobBaseForm()
            : base()
        {
            bmob = new BmobWindows();
            Bmob.initialize("f69acbf2dd96fbaefdf9fd9793e93f66", "9a956445ff932b7d3f59b81af28cbe2a ");
            BmobDebug.Register(msg => { Debug.WriteLine(msg); });

        }
        public BmobWindows Bmob
        {
            get { return bmob; }
        }

    }
}
