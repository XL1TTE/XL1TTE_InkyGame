using UnityEngine;
using Object = UnityEngine.Object;


namespace CMS
{
    public static class CMSUtil
    {
        public static T Load<T>(this string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public static Sprite LoadFromSpritesheet(string imageName, string spriteName)
        {
            Sprite[] all = Resources.LoadAll<Sprite>(imageName);

            foreach (var s in all)
            {
                if (s.name == spriteName)
                {
                    return s;
                }
            }
            return null;
        }
    }

}
