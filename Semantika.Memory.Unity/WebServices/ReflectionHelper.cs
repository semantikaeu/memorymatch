namespace Semantika.Memory.Unity.WebServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionHelper
    {
        public static List<PropertyInfo> GetProperties(Type type)
        {
#if NETFX_CORE
            return type.GetRuntimeProperties().ToList();
#else
            return type.GetProperties().ToList();
#endif
        }
    }
}
