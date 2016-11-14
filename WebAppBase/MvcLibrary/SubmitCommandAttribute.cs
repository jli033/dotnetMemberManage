using System;
using System.Reflection;
using System.Web.Mvc;

namespace AspMvcLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SubmitCommandAttribute : ActionMethodSelectorAttribute
    {
        private readonly string _submitName;
        private readonly string _submitValue;
        private static readonly AcceptVerbsAttribute InnerAttribute = new AcceptVerbsAttribute(HttpVerbs.Post);

        public SubmitCommandAttribute(string name) : this(name, string.Empty) { }
        public SubmitCommandAttribute(string name, string value)
        {
            _submitName = name;
            _submitValue = value;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            if (!InnerAttribute.IsValidForRequest(controllerContext, methodInfo))
                return false;

            // Form Value
            var submitted = controllerContext.RequestContext.HttpContext.Request.Form[_submitName];
            return string.IsNullOrEmpty(_submitValue)
                     ? !string.IsNullOrEmpty(submitted)
                     : string.Equals(submitted, _submitValue, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}