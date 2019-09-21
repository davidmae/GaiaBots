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

        public ActorBehaviour(ActorBase actor, Movable movement, StateMachine stateMachine)
        {
            Actor = actor;
            Movement = movement;
            StateMachine = stateMachine;
        }
    }
}