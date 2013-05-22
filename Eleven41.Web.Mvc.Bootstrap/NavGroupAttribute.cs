using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eleven41.Web.Mvc.Bootstrap
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class NavGroupAttribute : ActionFilterAttribute
	{
		string _groupName;

		public NavGroupAttribute(string groupName)
		{
			_groupName = groupName;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			filterContext.Controller.ViewBag.CurrentNavGroup = _groupName;

			base.OnActionExecuting(filterContext);
		}
	}
}