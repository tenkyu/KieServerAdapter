using System;

namespace KieServerAdapter
{
    /// <summary>
    /// Attribute to specify an explicit Drools (Java) fully qualified type name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Struct)
]
    public class DroolsTypeAttribute : Attribute
    {
        public string TypeName { get; set; }

        public DroolsTypeAttribute(string name)
        {
            TypeName = name;
        }
    }
}
