using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Eleven41.Web.Mvc.Bootstrap
{
	public class BootstrapLabel : IDisposable
	{

		private bool _disposed;
		private string _labelText;
		private readonly ViewContext _viewContext;
		private readonly TextWriter _writer;

		public BootstrapLabel(ViewContext viewContext, string labelText)
		{
			if (viewContext == null)
			{
				throw new ArgumentNullException("viewContext");
			}

			_viewContext = viewContext;
			_writer = viewContext.Writer;
			_labelText = labelText;
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
				_writer.Write(_labelText + "</label>");
			}
		}

		public void EndForm()
		{
			Dispose(true);
		}

	}
}
