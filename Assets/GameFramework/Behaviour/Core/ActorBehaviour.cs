using System;
using System.Linq;
using UnityEngine;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Interfaces;
using System.Collections;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.Movement.Core;

namespace Assets.GameFramework.Behaviour.Core
{
    public class ActorBehaviour : MonoBehaviour
    {
        public ActorBase Actor { get; set; }
        public Movable Movement { get; set; }
        public StateMachine StateMachine { get; set; }
        public HostilityBehaviour HostilityBehaviour { get; set; }

        public ActorBehaviour() { }

        public ActorBehaviour(ActorBase actor, Movable movement, StateMachine stateMachine)
        {
            Actor = actor;
            Movement = movement;
            StateMachine = stateMachine;
        }

        public static HostilityBehaviour PrepareHostilityBehaviour(ScriptableObject hostilityBehaviourData)
        {
            var hostilityBehaviour = new HostilityBehaviour();

            if (hostilityBehaviourData != null)
            {
                var value1 = hostilityBehaviourData.GetType().GetFields()[0].GetValue(hostilityBehaviourData);
                var value2 = hostilityBehaviourData.GetType().GetFields()[1].GetValue(hostilityBehaviourData);
                var value3 = hostilityBehaviourData.GetType().GetFields()[2].GetValue(hostilityBehaviourData);

                hostilityBehaviour.Value = (int)value1;
                hostilityBehaviour.IncrementSpeedFactor_MED = (float)value2;
                hostilityBehaviour.IncrementSpeedFactor_MAX = (float)value3;
            }
            else
            {
                hostilityBehaviour = null;
            }

            return hostilityBehaviour;
        }

    }

}