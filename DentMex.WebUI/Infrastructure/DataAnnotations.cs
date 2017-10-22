using System.ComponentModel.DataAnnotations;

namespace Student2Course.WebUI.Infrastructure
{
    public class IsTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool) value;
        }
    }
}