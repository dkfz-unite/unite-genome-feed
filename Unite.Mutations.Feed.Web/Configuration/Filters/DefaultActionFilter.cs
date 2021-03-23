﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Unite.Mutations.Feed.Web.Controllers.Extensions;

namespace Unite.Mutations.Feed.Web.Configuration.Filters
{
    public class DefaultActionFilter : IActionFilter
	{
		private readonly ILogger _logger;

		public DefaultActionFilter(ILogger<DefaultActionFilter> logger)
		{
			_logger = logger;
		}

        public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid(out var modelStateErrorMessage))
			{
				_logger.LogWarning(modelStateErrorMessage);

				context.Result = new BadRequestObjectResult(modelStateErrorMessage);
			}
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}
}