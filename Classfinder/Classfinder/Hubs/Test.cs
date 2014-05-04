using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classfinder.Hubs
{
    public class Test : Hub
    {
        public string TestStringRet()
        {
            return "123";
        }
    }
}