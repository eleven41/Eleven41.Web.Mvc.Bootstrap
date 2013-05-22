using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
	public class BootstrapModal : IDisposable
	{
		private bool _disposed;
		private readonly ViewContext _viewContext;
		private readonly TextWriter _writer;
		private BootstrapModalButtons _buttons;

		public BootstrapModal(ViewContext viewContext, BootstrapModalButtons buttons)
		{
			if (viewContext == null)
			{
				throw new ArgumentNullException("viewContext");
			}

			_viewContext = viewContext;
			_writer = viewContext.Writer;
			_buttons = buttons;
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
				_writer.Write("</div>"); // Close the body

				if (_buttons != BootstrapModalButtons.OmitFooter)
				{
					_writer.Write("<div class=\"modal-footer\">"); // Start the footer

					// Write the required buttons
					switch (_buttons)
					{
						case BootstrapModalButtons.Cancel:
							_writer.Write("<button class=\"btn\" type=\"button\" data-dismiss=\"modal\">Cancel</button>");
							break;

						case BootstrapModalButtons.Close:
							_writer.Write("<button class=\"btn\" type=\"button\" data-dismiss=\"modal\">Close</button>");
							break;

						case BootstrapModalButtons.OK:
							_writer.Write("<button class=\"btn\" type=\"button\" data-dismiss=\"modal\">OK</button>");
							break;
					}

					_writer.Write("</div>"); // Close the footer
				}

				_writer.Write("</div>"); // Close the modal
			}
		}

		public void EndForm()
		{
			Dispose(true);
		}
	}
}
