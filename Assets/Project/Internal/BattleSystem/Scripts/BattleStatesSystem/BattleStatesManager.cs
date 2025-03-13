using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Project.Internal.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Internal.BattleSystem.States
{
    public static class BattleStatesManager
    {
        public static BaseBattleState CurrentState;
        public static BaseBattleState PreviousState;

        public static Dictionary<Type, BaseBattleState> AllStates = new();

        public static IEnumerator Init()
        {

            var states_types = ReflectionUtility.GetSubclasses<BaseBattleState>();

            foreach (var type in states_types)
            {
                var state = Activator.CreateInstance(type) as BaseBattleState;

                AllStates.TryAdd(type, state);
            }
            yield return null;
        }


        public static void ToPreviousState()
        {
            var temp = CurrentState;
            CurrentState = PreviousState;

            PreviousState = temp;
        }
        public static StateType SetCurrentState<StateType>() where StateType : BaseBattleState
        {
            if (AllStates.TryGetValue(typeof(StateType), out var state))
            {
                PreviousState = CurrentState;
                CurrentState = state;

                return CurrentState as StateType;
            }
            return null;
        }
    }
}
