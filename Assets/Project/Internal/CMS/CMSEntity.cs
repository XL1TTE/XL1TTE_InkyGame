using System;
using System.Collections.Generic;

namespace CMS
{
    public partial class CMSEntity
    {
        public string id;

        public List<EntityComponentDefinition> components = new List<EntityComponentDefinition>() { new AnythingTag() };

        public T Define<T>() where T : EntityComponentDefinition, new()
        {
            var t = Get<T>();
            if (t != null)
                return t;

            var entity_component = new T();
            components.Add(entity_component);
            return entity_component;
        }

        public bool Is<T>(out T unknown) where T : EntityComponentDefinition, new()
        {
            unknown = Get<T>();
            return unknown != null;
        }

        public bool Is<T>() where T : EntityComponentDefinition, new()
        {
            return Get<T>() != null;
        }

        public bool Is(Type type)
        {
            return components.Find(m => m.GetType() == type) != null;
        }

        public T Get<T>() where T : EntityComponentDefinition, new()
        {
            return components.Find(m => m is T) as T;
        }
    }
}
