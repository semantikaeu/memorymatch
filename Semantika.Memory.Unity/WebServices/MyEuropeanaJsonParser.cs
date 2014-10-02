namespace Semantika.Memory.Unity.WebServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MyEuropeanaJsonParser : IMyEuropeanaParser
    {
        public T Parse<T>(string content) where T : class 
        {
            if (typeof(T) == typeof(List<string>))
            {
                content = content.Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty);
                return content.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() as T;
            }

            return default(T);
        }
    }
}
