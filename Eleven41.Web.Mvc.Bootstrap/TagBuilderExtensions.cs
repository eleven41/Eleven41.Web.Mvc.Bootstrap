using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
	public static class TagBuilderExtensions
	{
		public static void AddOrMergeAttribute(this TagBuilder self, string attributeName, string value)
		{
			if (self.Attributes.ContainsKey(attributeName))
				self.Attributes[attributeName] = self.Attributes[attributeName] + " " + value;
			else
				self.Attributes.Add(attributeName, value);
		}

		public static void AddAttributes(this TagBuilder tag, object htmlAttributes)
		{
			if (htmlAttributes == null)
				return;

			Type type = htmlAttributes.GetType();
			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				if (!property.CanRead)
					continue;

				string propertyName = property.Name;
				if (String.IsNullOrEmpty(propertyName))
					continue;

				propertyName = propertyName.Replace("_", "-");

				var value = property.GetValue(htmlAttributes, null);

				tag.MergeAttribute(propertyName, value.ToString(), true);
			}
		}
	}
}
