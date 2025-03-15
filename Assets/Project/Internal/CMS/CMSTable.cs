using System.Collections.Generic;
using System.Linq;

namespace CMS
{
    public class CMSTable<T> where T : CMSEntity, new()
    {
        Dictionary<string, T> dict = new Dictionary<string, T>();

        public void Add(T inst)
        {
            if (inst.id == null)
                inst.id = E.Id(inst.GetType());

            dict.Add(inst.id, inst);
        }

        public T New(string id)
        {
            var t = new T();
            t.id = id;
            dict.Add(id, t);
            return t;
        }

        public List<T> GetAll()
        {
            return dict.Values.ToList();
        }

        public T FindById(string id)
        {
            return dict.GetValueOrDefault(id);
        }

        public T2 FindByType<T2>() where T2 : T
        {
            foreach (var v in dict.Values)
                if (v is T2 v2)
                    return v2;
            return null;
        }
    }

}
