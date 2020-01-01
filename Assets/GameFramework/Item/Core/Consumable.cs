using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.UI;
using System;
using UnityEngine;

namespace Assets.GameFramework.Item.Core
{
    public class Consumable<TStatus> : GFrameworkEntityBase, IConsumable
        where TStatus : StatusBase
    {
        public int Durability;
        public float BonusValue;

        public StatusBase StatusModified { get => GetStatusModified(); set => StatusModified = value; }
        public event Func<StatusBase> OnGetStatusModified;
        public StatusBase GetStatusModified()
        {
            if (OnGetStatusModified != null)
                return OnGetStatusModified();
            return null;
        }

        public virtual void Detect(ActorBase actor, SenseBase senseBase)
        {
            if (this == null || this.ToString() == "null")
                return;

            if (!actor.DetectableQueue.Contains(this))
                actor.DetectableQueue.Add(this);

            if (!actor.Behaviour.StateMachine.CurrentState.IsGoingToItem)
            {
                actor.transform.LookAt(transform.position);
                actor.Behaviour.StateMachine.NextState = Behaviour.Core.StateMachine_BaseStates.GoingToItem;
                actor.Behaviour.StateMachine.Update();
            }
        }

        public GameObject GetGameObject() => gameObject;
        public Vector3 GetPosition() => transform.position;
        public int GetCurrentDurability() => Durability;
        public float GetBonusValue() => BonusValue;
        public int MinusOneDurabilityPoint() => Durability == -1 ? Durability : --Durability;

        

        
    }


}
