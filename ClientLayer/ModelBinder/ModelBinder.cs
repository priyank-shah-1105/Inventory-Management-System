using System.Diagnostics;
using System.Web;
using System.Web.Mvc;

namespace Client
{
    public class TrimModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                var stringValue = (string)value;
                if (!string.IsNullOrEmpty(stringValue))
                {
                    stringValue = stringValue.Trim();
                }

                value = stringValue;
            }

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }

    public class DefaultModelBinderWithHtmlValidation : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            catch (HttpRequestValidationException)
            {
                Trace.TraceWarning("Ilegal characters were found in field {0}", bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelName);
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("Ilegal characters were found in field {0}", bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelName));
            }

            IUnvalidatedValueProvider provider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
            if (provider == null) return null;

            var result = provider.GetValue(bindingContext.ModelName, true);
            Debug.Assert(result != null, "result is null");

            return result.AttemptedValue;
        }
    }
}