using System;

namespace EcommerceCRUD.Attributes
{
        [AttributeUsage(AttributeTargets.Method)]
        public class AllowAnonymousAttribute : Attribute
        { }
}
