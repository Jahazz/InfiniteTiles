using System;
using System.Linq;
using System.Collections.Generic;

namespace CodeBase
{
    public static class Extensions
    {
        public static EnumType GetRandomEnum<EnumType> (this EnumType enumType) where EnumType : struct, IConvertible
        {
            Array values = Enum.GetValues(enumType.GetType());
            return (EnumType)values.GetValue(new Random().Next(0, values.Length));
        }

        public static T RandomElement<T> (this IEnumerable<T> enumerable)
        {
            int index = new Random().Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
    }
}
