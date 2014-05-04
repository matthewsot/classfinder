using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Classfinder.Controllers
{
    [AllowAnonymous]
    public class HooksController : Controller
    {
        public ActionResult PostmarkInbound()
        {
            return View();
        }
    }
}
