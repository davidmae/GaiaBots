using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameFramework.Behaviour.Core;

namespace Assets.GameFramework.Common
{
    public class StatesDictionary
    {
        public Dictionary<StateMachine_BaseStates, Func<IEnumerator>> Dictionary;

        public StatesDictionary()
        {
            Dictionary = new Dictionary<StateMachine_BaseStates, Func<IEnumerator>>();
        }

        public Func<IEnumerator> this[StateMachine_BaseStates key] { get => Dictionary[key]; }

        public void AddState(StateMachine_BaseStates baseState, Func<IEnumerator> stateAction)
        {
            Dictionary.Add(baseState, stateAction);
        }

    }
}
