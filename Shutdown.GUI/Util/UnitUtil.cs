using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shutdown.GUI.Data.Unit;

namespace Shutdown.GUI.Util
{
    public sealed class UnitUtil
    {
        private readonly ReflectionExtractor _reflectionExtractor;

        public UnitUtil(ReflectionExtractor reflectionExtractor)
        {
            _reflectionExtractor = reflectionExtractor;
        }

        public IEnumerable<T> GetAllValues<T>() 
            where T : UnitBase<T> 
            => _reflectionExtractor
                .GetAllStaticFields<T>(typeof(T), BindingFlags.Public);
        
        public IEnumerable<T> GetAllPriorityValues<T>() 
            where T : PriorityUnitBase<T> 
            => _reflectionExtractor
                .GetAllStaticFields<T>(typeof(T), BindingFlags.Public)
                .OrderBy(e => e.Priority);
    }
}