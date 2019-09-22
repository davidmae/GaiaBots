using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameFramework.Item.Core
{
    public class Consumable : MonoBehaviour, IConsumable, IDetectable
    {
        public string satiety;

        public virtual void Detect(ActorBase actor)
        {
            actor.Behaviour.Movement.MoveToPosition(transform.position);
            actor.Behaviour.StateMachine.UpdateStates(gotoEat: true);

            //currentActor.Behaviour.StateMachine.NextAction = currentActor.Behaviour.StateMachine.IsEatingRoutine;
        }

    }
}
