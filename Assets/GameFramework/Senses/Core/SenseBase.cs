using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Interfaces;
using UnityEngine;

namespace Assets.GameFramework.Senses.Core
{
    public class SenseBase : MonoBehaviour, ISense
    {
        protected ActorBase Actor;

        public virtual void Detect(ActorBase actor, IDetectable detectable)
        {
            if (detectable == null)
                return;

            if (actor == null)
                return;

            if (actor.Behaviour.StateMachine.CurrentState.IsStayFront)
                return;

            if (actor.Behaviour.StateMachine.CurrentState.IsFighting)
                return;

            if (detectable is IConsumable)
            {
                var consumable = (IConsumable)detectable;
                var statusFromConsumable = consumable.StatusModified;

                if (actor.IsFull(statusFromConsumable))
                    return;
            }

            detectable.Detect(actor);
        }
    }
}
