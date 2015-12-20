using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;

namespace WorldServer.Emu.Extensions
{
    public static class DaoExt
    {
        public static FluentMappingsContainer AddFromNamespaceOf<T>(
            this FluentMappingsContainer fmc)
        {
            string ns = typeof(T).Namespace;
            IEnumerable<Type> types = typeof(T).Assembly.GetExportedTypes()
                .Where(t => t.Namespace == ns)
                .Where(x => IsMappingOf<IMappingProvider>(x) ||
                            IsMappingOf<IIndeterminateSubclassMappingProvider>(x) ||
                            IsMappingOf<IExternalComponentMappingProvider>(x) ||
                            IsMappingOf<IFilterDefinition>(x));

            foreach (Type t in types)
            {
                fmc.Add(t);
            }

            return fmc;
        }

        /// <summary>
        /// Private helper method cribbed from FNH source (PersistenModel.cs:151)
        /// </summary>
        private static bool IsMappingOf<T>(Type type)
        {
            return !type.IsGenericType && typeof (T).IsAssignableFrom(type);
        }

    }
}
