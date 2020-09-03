using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shutdown.GUI.Util
{
    public sealed class ReflectionExtractor
    {
        public IEnumerable<T> GetAllStaticFields<T>(Type type, BindingFlags flag)
        {
            var fieldType = typeof(T);

            return type.GetFields(flag | BindingFlags.Static)
                .Where(e => e.DeclaringType == fieldType)
                .Select(e => (T) e.GetValue(null));
        }
    }
}