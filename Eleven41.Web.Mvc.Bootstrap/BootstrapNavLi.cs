using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Eleven41.Web.Mvc.Bootstrap
{
	public class NavLi : IDisposable
	{

		private bool _disposed;
		private readonly ViewContext _viewContext;
		private readonly TextWriter _writer;

		public NavLi(ViewContext viewContext)
		{
			if (viewContext == null)
			{
				throw new ArgumentNullException("viewContext");
			}

			_viewContext = viewContext;
			_writer = viewContext.Writer;
		}

		public void Dispose()
		{
			Dispose(true /* disposing */);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;
				_writer.Write("</li>");
			}
		}

		public void EndForm()
		{
			Dispose(true);
		}

	}
}
