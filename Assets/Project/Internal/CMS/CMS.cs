using System;
using System.Collections.Generic;
using Project.Internal.Utilities;


namespace CMS
{
    public static class CMS
    {
        static CMSTable<CMSEntity> all = new CMSTable<CMSEntity>();

        static bool isInit;

        public static void Init()
        {
            if (isInit)
                return;
            isInit = true;

            AutoAdd();
        }

        static void AutoAdd()
        {
            var subs = ReflectionUtility.GetSubclasses<CMSEntity>();
            foreach (var subclass in subs)
                all.Add(Activator.CreateInstance(subclass) as CMSEntity);
        }

        public static T Get<T>(string entity_id = null) where T : CMSEntity
        {
            if (entity_id == null)
                entity_id = E.Id<T>();
            var findById = all.FindById(entity_id) as T;

            if (findById == null)
            {
                throw new Exception("unable to resolve entity id '" + entity_id + "'");
            }

            return findById;
        }


        public static T GetComponent<T>(string entity_id = null) where T : EntityComponentDefinition, new()
        {
            return Get<CMSEntity>(entity_id).Get<T>();
        }

        public static List<T> GetAll<T>() where T : CMSEntity
        {
            var allSearch = new List<T>();

            foreach (var a in all.GetAll())
                if (a is T)
                    allSearch.Add(a as T);

            return allSearch;
        }

        public static List<(CMSEntity e, T tag)> GetAllComponents<T>() where T : EntityComponentDefinition, new()
        {
            var allSearch = new List<(CMSEntity, T)>();

            foreach (var a in all.GetAll())
                if (a.Is<T>(out var t))
                    allSearch.Add((a, t));

            return allSearch;
        }

        public static void Unload()
        {
            isInit = false;
            all = new CMSTable<CMSEntity>();
        }
    }



    public static class E
    {
        public static string Id(Type getType)
        {
            return getType.FullName;
        }

        public static string Id<T>() where T : CMSEntity
        {
            return ID<T>.Get();
        }
    }

    static class ID<T> where T : CMSEntity
    {
        static string cache;

        public static string Get()
        {
            if (cache == null)
                cache = typeof(T).FullName;
            return cache;
        }
    }
}


