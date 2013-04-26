using System;
using System.Web.Mvc;
using WebApp4.Infrastructure;

namespace WebApp4
{
	/// <summary>
	/// Logs errors using NLog.Mvc logger
	/// </summary>
	public class NLogMvcHandleErrorAttribute: HandleErrorAttribute
	{
		private readonly ILogger logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="NLogMvcHandleErrorAttribute"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public NLogMvcHandleErrorAttribute(ILogger logger)
		{
			this.logger = logger;
		}

		/// <summary>
		/// Called when an exception occurs.
		/// </summary>
		/// <param name="filterContext">The action-filter context.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="filterContext"/> parameter is null.</exception>
		public override void OnException(ExceptionContext filterContext)
		{
            this.logger.Error(string.Format("意外捕获错误 {0}", filterContext.Exception.Message), filterContext.Exception);
			base.OnException(filterContext);
		}
	}
}
