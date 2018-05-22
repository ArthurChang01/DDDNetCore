using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using System.Linq;

namespace Fundamentals.Filters
{
    public class DTOValidateFilter : IActionFilter
    {
        private ILogger _logger;
        private IHostingEnvironment _env;

        public DTOValidateFilter(IHostingEnvironment env)
        {
            this._logger = Serilog.Log.ForContext<DTOValidateFilter>();
            this._env = env;
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// Action執行前觸發事件方法
        /// </summary>
        /// <param name="context"></param>
        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            string errMsg = TransformModelStateToString(context.ModelState);
            this._logger.Error(errMsg);

            context.Result = new BadRequestObjectResult(context.ModelState);
        }

        /// <summary>
        /// 轉換ModelState物件回字串
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private string TransformModelStateToString(ModelStateDictionary modelState)
        {
            var errorList = modelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToArray();
            return string.Join("\t\n", errorList);
        }
    }
}