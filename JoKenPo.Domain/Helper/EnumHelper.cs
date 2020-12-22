using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace JoKenPo.Domain.Helper
{
    public static class EnumHelper
    {
        public static string Description(this System.Enum type)
        {
            var fieldInfo = type.GetType().GetRuntimeField(type.ToString());
            if (fieldInfo == null)
                return type.ToString();
            var atributo = fieldInfo.GetCustomAttribute<DisplayAttribute>();
            if (atributo != null)
                return atributo.Description;
            var nameAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return nameAttribute != null ? nameAttribute.Description : type.ToString();
        }
    }
}