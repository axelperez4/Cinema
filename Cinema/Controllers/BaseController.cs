using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cinema.Controllers
{
    public class BaseController : Controller
    {
        public Business business = new Business();
    }
}