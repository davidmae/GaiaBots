using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.UI;
using System;
using System.Collections.Generic;
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

        public virtual bool Detect(ActorBase actor, SenseBase senseBase)
        {
            if (this == null || this.ToString() == "null")
                return false;

            if (!actor.DetectableQueue.Contains(this))
                actor.DetectableQueue.Add(this);

            if (!actor.Behaviour.StateMachine.CurrentState.IsGoingToItem)
            {
                actor.transform.LookAt(transform.position);
                actor.Behaviour.StateMachine.NextState = Behaviour.Core.StateMachine_BaseStates.GoingToItem;
                actor.Behaviour.StateMachine.Update();
            }

            return true;
        }

        public Vector3 GetPosition() => transform.position;
        public int GetCurrentDurability() => Durability;
        public float GetBonusValue() => BonusValue;
        public int MinusOneDurabilityPoint() => Durability == -1 ? Durability : --Durability;
        
        public override bool Equals(IGFrameworkEntityBase item)
        {
            var consumable = item as IConsumable;

            if (consumable == null) return false;

            return
                GetType().BaseType == consumable.GetType().BaseType &&
                GetOriginalName() == consumable.GetOriginalName() &&
                GetBonusValue() == consumable.GetBonusValue() &&
                GetCurrentDurability() == consumable.GetCurrentDurability();
        }

        public override List<KeyValuePair<string, object>> GetEntityFields()
        {
            return new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("Durability", Durability),
                new KeyValuePair<string, object>("BonusValue", BonusValue)
            };
        }

        public override GFrameworkEntityBase DeepCopy()
        {
            var item = Instantiate(this);
            item.gameObject.SetActive(true);
            return item;
        }
    }

}
