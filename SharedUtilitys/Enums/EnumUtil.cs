using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedUtilitys.Enums
{
    public static class EnumUtil<T>
    {
        public static bool IsDefined(int n)
        {
            return Enum.IsDefined(typeof(T), n);
        }

        public class EnumerateEnum
        {
            public IEnumerator<T> GetEnumerator()
            {
                return Enum.GetValues(typeof(T)).Cast<T>().GetEnumerator();
            }
        }

        public static EnumerateEnum Enumerate()
        {
            return new EnumerateEnum();
        }
    }
}
