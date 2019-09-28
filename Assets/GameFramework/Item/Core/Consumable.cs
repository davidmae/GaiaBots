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
    public class Consumable : MonoBehaviour, IConsumable
    {
        public int satiety;

        public event Action OnUpdateSatiety;


        public virtual void Detect(ActorBase actor)
        {
            if (this == null || this.ToString() == "null")
                return;

            if (!actor.DetectablesQueue.Contains(this))
                actor.DetectablesQueue.Enqueue(this);

            if (!actor.Behaviour.StateMachine.CurrentState.IsGoingToEat)
            {
                actor.Behaviour.Movement.MoveToPosition(transform.position);
                actor.Behaviour.StateMachine.UpdateStates(gotoEat: true);
            }
        }

        public int GetSacietyPoints()
        {
            return satiety;
        } 

        public int MinusOneSacietyPoint()
        {
            return satiety == -1 ? satiety : --satiety;
        }

        private void InteractionUpdate()
        {
            if (OnUpdateSatiety != null)
                OnUpdateSatiety();
        }

        public void UseItem()
        {
            if (!IsInvoking("InteractionUpdate"))
                InvokeRepeating("InteractionUpdate", 0f, 1f);
        }

        public void LeaveItem()
        {
            CancelInvoke("InteractionUpdate");
        }

        public void DestroyItem()
        {
            Destroy(gameObject);
        }


    }
}
