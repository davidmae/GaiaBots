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
        public int satiety;

        public event Action OnUpdateSatiety;

        public virtual void Detect(ActorBase actor)
        {
            actor.Behaviour.Movement.MoveToPosition(transform.position);
            actor.Behaviour.StateMachine.UpdateStates(gotoEat: true);
            actor.CurrentItem = this;

            //currentActor.Behaviour.StateMachine.NextAction = currentActor.Behaviour.StateMachine.IsEatingRoutine;
        }

        public int GetSacietyPoints()
        {
            return satiety;
        } 

        public int MinusOneSacietyPoint()
        {
            if (OnUpdateSatiety != null)
                OnUpdateSatiety();

            return satiety--;
        }

        public void UseItem()
        {
            InvokeRepeating("MinusOneSacietyPoint", 0f, 1f);
        }

        public void LeaveItem()
        {
            CancelInvoke("MinusOneSacietyPoint");
        }

        public void DestroyItem()
        {
            Destroy(gameObject);
        }


    }
}
