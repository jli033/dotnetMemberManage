using System;
using SharedUtilitys.Enums;

namespace SharedUtilitys.Attributes
{
    [AttributeUsageAttribute(AttributeTargets.Class)]
    public class RegistryLocationAttribute : Attribute
    {
        public RegistryLocationEnum RegistryLocation { get; set; }
        public string ParentKey { get; set; }
    }
}
