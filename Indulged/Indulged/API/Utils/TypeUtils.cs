using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Indulged.API.Utils
{
    public static class TypeUtils
    {
        public static bool HasImplementedInterface(this Type type, Type _interface)
        {
            Type[] allInterfaces = type.GetTypeInfo().ImplementedInterfaces.ToArray();
            if (allInterfaces != null)
            {
                if (allInterfaces.Contains(_interface))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
