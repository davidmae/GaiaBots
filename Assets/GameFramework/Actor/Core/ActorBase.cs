using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
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
        public Queue<IDetectable> DetectablesQueue { get; set; } = new Queue<IDetectable>();
        

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

            // Solo encara al actor si entra en su rango de vision
            // Evita que el otro actor tambien lo encare si su vision es menor...

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

        public T GetCurrentDetectable<T>() where T : IDetectable
        {
            return (T)DetectablesQueue?.Peek();
        }

        public void RestoreHungry()
        {
            StatusInstances[StatusTypes.Hungry].UpdateStatus(1);
        }
    }
}
