using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameFramework.Actor.Core
{
    public class ActorBase : MonoBehaviour, IDetectableDynamic
    {
        public ActorBehaviour Behaviour { get; set; }
        public IDictionary<StatusTypes, StatusBase> StatusInstances { get; set; }
        public IItem CurrentItem { get; set; }


        [Header("Debugging fields")]

        // -------- Debugging on inspector --------
        public States CurrentState;
        public List<StatusBase> ListStatus;
        // ----------------------------------------

        public virtual void Detect(ActorBase actor)
        {
            var visionDistance = actor.GetComponent<ConeCollider>().Distance;
            var detectDistance = Vector3.Distance(actor.transform.position, this.transform.position);

            if (visionDistance < detectDistance)
                return;

            actor.transform.LookAt(this.transform);
            actor.Behaviour.Movement.MoveToPosition(this.transform.position);
            actor.Behaviour.StateMachine.UpdateStates(gotoFight: true);

            //Behaviour.StateMachine.NextAction = Behaviour.StateMachine.IsFightingRoutine;

            // ----------------------------- For debugging ------------------------------
            //var marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            //marker.transform.localScale = new Vector3(.1f, 20f, .1f);
            //GameObject.Instantiate(marker, transform.position, Quaternion.LookRotation(Vector3.zero));
            //GameObject.Destroy(marker, 1f);

            //Debug.Log($"{name} has detected to {currentActor.name}");
        }

        public T GetCurrentItem<T>() where T : IItem
        {
            return (T)CurrentItem;
        }

        public void RestoreHungry()
        {
            StatusInstances[StatusTypes.Hungry].UpdateStatus(1);
        }
    }
}
