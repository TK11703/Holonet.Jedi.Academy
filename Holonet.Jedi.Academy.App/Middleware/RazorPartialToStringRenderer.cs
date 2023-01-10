using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Holonet.Jedi.Academy.App.Middleware
{
    public class RazorPartialToStringRenderer : IRazorPartialToStringRenderer
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
		private readonly IHttpContextAccessor _contextAccessor;

		public RazorPartialToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
			IHttpContextAccessor contextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
			_contextAccessor = contextAccessor;
		}
        public async Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model)
        {
			if(_contextAccessor == null || _contextAccessor.HttpContext == null) 
			{
				throw new NullReferenceException("IHttpContextAccessor or HttpContext was null.");
			}
			var actionContext = new ActionContext(_contextAccessor.HttpContext, _contextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());

			await using var sw = new StringWriter();
			var viewResult = FindView(actionContext, partialName);

			if (viewResult == null)
			{
				throw new ArgumentNullException($"{partialName} does not match any available view.");
			}

			var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
			{
				Model = model
			};

			var viewContext = new ViewContext(
				actionContext,
				viewResult,
				viewDictionary,
				new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
				sw,
				new HtmlHelperOptions()
			);

			await viewResult.RenderAsync(viewContext);
			return sw.ToString();
		}
        private IView FindView(ActionContext actionContext, string partialName)
        {
			var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: partialName, isMainPage: false);
			if (getViewResult.Success)
			{
				return getViewResult.View;
			}

			var findViewResult = _viewEngine.FindView(actionContext, partialName, isMainPage: false);
			if (findViewResult.Success)
			{
				return findViewResult.View;
			}

			var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
			var errorMessage = string.Join(
				Environment.NewLine,
				new[] { $"Unable to find view '{partialName}'. The following locations were searched:" }.Concat(searchedLocations));

			throw new InvalidOperationException(errorMessage);
		}
    }
}
