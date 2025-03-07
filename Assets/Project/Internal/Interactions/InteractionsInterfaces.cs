using System.Collections;
using System.Collections.Generic;
using Project.Internal.BattleSystem;
using Project.Internal.System;
using UnityEngine;

namespace Project.Internal.Interactions
{

    #region GameInit

    public interface IOnGameInitStart
    {
        public IEnumerator OnGameInit(GameInitializer context);
    }

    public interface IOnGameInitEnd
    {
        public IEnumerator OnGameInitEnd(GameInitializer context);
    }
    #endregion

    #region Battle
    public interface IOnBattleInitStart
    {
        public IEnumerator OnInitStart(BattleManager context);
    }

    public interface IOnBattleReady
    {
        public IEnumerator OnBattleReady(BattleManager context);
    }
    #endregion

}
