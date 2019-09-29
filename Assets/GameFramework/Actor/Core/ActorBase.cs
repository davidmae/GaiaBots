using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameFramework.Actor.Core
{
    public class PriorityQueue<T> : List<T> where T : IDetectable
    {
    }

    public class ActorBase : MonoBehaviour, IDetectableDynamic
    {
        public ActorBehaviour Behaviour { get; set; }
        public IDictionary<StatusTypes, StatusBase> StatusInstances { get; set; }
        public PriorityQueue<IDetectable> DetectableQueue { get; set; } = new PriorityQueue<IDetectable>();


        public event Action OnUpdateStatus;

        [Header("Debugging fields")]

        // -------- Debugging on inspector --------
        public States CurrentState;
        public List<StatusBase> ListStatus;
        // ----------------------------------------

        public virtual void Detect(ActorBase actor)
        {
            if (this == null || this.ToString() == "null")
                return;

            var visionDistance = actor.GetComponent<ConeCollider>().Distance;
            var detectDistance = Vector3.Distance(actor.transform.position, this.transform.position);

            if (visionDistance < detectDistance)
                return;

            if (!actor.DetectableQueue.Contains(this))
                actor.DetectableQueue.Add(this);


            // Solo encara al actor si entra en su rango de vision
            // Evita que el otro actor tambien lo encare si su vision es menor...

            actor.transform.LookAt(this.transform);
            actor.Behaviour.Movement.MoveToPosition(this.transform.position);
            actor.Behaviour.StateMachine.UpdateStates(gotoFight: true);


            //-----------------------------For debugging------------------------------
            //var marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            //marker.transform.localScale = new Vector3(.1f, 20f, .1f);
            //GameObject.Instantiate(marker, transform.position, Quaternion.LookRotation(Vector3.zero));
            //GameObject.Destroy(marker, 1f);

            //Debug.Log($"{name} has detected to {currentActor.name}");
        }

        public T GetCurrentDetectable<T>() where T : IDetectable
        {
            return (T)DetectableQueue.Find(d => d.GetType().BaseType == typeof(T));
        }

        public void RestoreHungry()
        {
            int itemSatiety = GetCurrentDetectable<Consumable>().MinusOneSacietyPoint();

            if (itemSatiety >= 0)
                StatusInstances[StatusTypes.Hungry].UpdateStatus(1);

            // Debug
            //Debug.Log($"{this.name} --- {GetCurrentDetectable<IConsumable>().GetSacietyPoints()}" +
            //    $" --- saciety actor: {StatusInstances[StatusTypes.Hungry].Current}");
        }

        public void DamageHealth()
        {
            var targetActor = GetCurrentDetectable<ActorBase>();
            var targetValue = targetActor.MinusOneCurrentPoint();

            if (targetValue >= 0)
                targetActor.StatusInstances[StatusTypes.Health].UpdateStatus(-1);

            // Debug
            //Debug.Log($"target -> {GetCurrentDetectable<ActorBase>().name} --- {GetCurrentDetectable<ActorBase>().StatusInstances[StatusTypes.Health].Current}" +
            //    $" --- health actor {this.name}: {StatusInstances[StatusTypes.Health].Current}");
        }

        public void ProcessStatus()
        {
            if (!IsInvoking("InteractionUpdate"))
                InvokeRepeating("InteractionUpdate", 0f, 1f);
        }

        public void EndProcessStatus()
        {
            CancelInvoke("InteractionUpdate");
        }

        private void InteractionUpdate()
        {
            if (OnUpdateStatus != null)
                OnUpdateStatus();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public int MinusOneCurrentPoint()
        {
            var current = StatusInstances[StatusTypes.Health].Current;
            return current == -1 ? current : --current;
        }
    }
}
