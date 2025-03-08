using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Internal.Utilities
{
    public static class SpawnerUtility
    {
        public static void SpawnItemIn(GameObject item, Transform Spawn, bool activateOnSpawn = false)
        {
            item.transform.SetParent(Spawn, false);
            if (activateOnSpawn)
            {
                item.SetActive(true);
            }
        }
    }
}

