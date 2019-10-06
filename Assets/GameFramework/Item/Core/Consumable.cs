using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using UnityEngine;

namespace Assets.GameFramework.Item.Core
{
    public class Consumable<TStatus> : GFrameworkEntityBase, IConsumable
        where TStatus : StatusBase
    {
        public int Value;

        public virtual void Detect(ActorBase actor)
        {
            if (this == null || this.ToString() == "null")
                return;

            if (!actor.DetectableQueue.Contains(this))
                actor.DetectableQueue.Add(this);

            if (!actor.Behaviour.StateMachine.CurrentState.IsGoingToItem)
            {
                actor.Behaviour.StateMachine.NextState = Behaviour.Core.StateMachine_BaseStates.GoingToItem;
                actor.Behaviour.StateMachine.Update();
            }
        }

        public Vector3 GetPosition() => transform.position;

        public int GetCurrentPoints()
        {
            return Value;
        }

        public int MinusOnePoint()
        {
            return Value == -1 ? Value : --Value;
        }

    }

    
}
