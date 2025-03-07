using System;
using System.Collections;
using System.Collections.Generic;
using Project.Internal.Utilities;
using UnityEngine;

namespace Project.Internal.Interactions
{
    public class InteractionManager
    {
        public List<InteractionBase> all = new();

        public void Init()
        {
            var types = ReflectionUtility.GetSubclasses<InteractionBase>();
            foreach (var t in types)
            {
                all.Add((InteractionBase)Activator.CreateInstance(t));
            }
        }

        public List<T> FindAllOf<T>()
        {
            return InteractionsCache<T>.GetAll(this);
        }
    }


    public static class InteractionsCache<T>
    {
        private static List<T> all;

        public static List<T> GetAll(InteractionManager Intmanager)
        {
            if (all != null)
            {
                return all;
            }

            all = new List<T>(64);
            foreach (var interaction in Intmanager.all)
            {
                if (interaction is T t)
                {
                    all.Add(t);
                }
            }
            all.Sort((a, b) => (b as InteractionBase).Priority() - (a as InteractionBase).Priority());
            return all;
        }
    }
}
