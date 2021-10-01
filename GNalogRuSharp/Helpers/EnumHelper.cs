using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNalogRuSharp.Helpers
{
    /// <summary>
    /// Помощник по получению значений <see cref="DescriptionAttribute"/> у значений перечислений.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Получить описание значение перечисления.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
            {
                return (attributes.First() as DescriptionAttribute).Description;
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
        }

        /// <summary>
        /// Получить колекцию значений-описание всех значений перечисления.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type t)
        {
            if (!t.IsEnum)
            {
                throw new ArgumentException($"{nameof(t)} не является перечислением.");
            }

            return Enum.GetValues(t).Cast<Enum>().Select((e) => new ValueDescription() { Value = e, Description = e.GetDescription() }).ToList();
        }
    }
}
