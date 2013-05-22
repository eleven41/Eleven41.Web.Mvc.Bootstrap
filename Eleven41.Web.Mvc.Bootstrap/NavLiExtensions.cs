using Eleven41.Web.Mvc.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
	public static class NavLiExtensions
	{
		public static NavLi BeginBootstrapNavLi<TModel>(this HtmlHelper<TModel> html, string navGroupName)
		{
			return BeginBootstrapNavLi(html, navGroupName, null);
		}

		public static NavLi BeginBootstrapNavLi<TModel>(this HtmlHelper<TModel> html, string navGroupName, string className)
		{
			TagBuilder tag = new TagBuilder("li");

			if (html.ViewContext.Controller.ViewBag.CurrentNavGroup == navGroupName)
			{
				tag.Attributes.Add("class", "active");
			}

			if (!String.IsNullOrEmpty(className))
			{
				tag.AddOrMergeAttribute("class", className);
			}

			html.ViewContext.Writer.Write(tag.ToString(TagRenderMode.StartTag));

			return new NavLi(html.ViewContext);
		}
	}
}
