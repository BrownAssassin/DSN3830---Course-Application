using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace GameServer001.Controller
{
    abstract class BaseController
    {
        protected RequestCode requestCode = RequestCode.None;
        public RequestCode RequestCode
        {
            get { return requestCode; }
        }
    }
}
