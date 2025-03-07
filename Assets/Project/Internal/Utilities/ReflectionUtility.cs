using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Project.Internal.Utilities
{
    public static class ReflectionUtility
    {
        public static Type[] GetSubclasses<T>()
        {
            var types = Assembly.GetExecutingAssembly().DefinedTypes;

            return types.Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract).ToArray();
        }
    }
}
