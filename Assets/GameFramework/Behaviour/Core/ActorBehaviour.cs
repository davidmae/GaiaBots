using Assets.GameFramework.Status.Core;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Interfaces;
using Assets.GameFramework.Movement.Core;

namespace Assets.GameFramework.Behaviour.Core
{
    public class ActorBehaviour : IActorBehaviour
    {
        public ActorBase Actor { get; set; }
        public IMovable Movement { get; set; }

        public IList<StatusBase> StatusBaseList { get; set; }

        public void SetEvaluateStatusAction<T>(Action<ActorBase> evaluate)
            where T : StatusBase
        {
            StatusBaseList
                .Where(status => status is T)
                .FirstOrDefault()
                .onStatusChange += evaluate;
        }

        public void EvaluateStatus()
        {
            foreach (var status in StatusBaseList)
            {
                status.EvaluateStatus(Actor);
            }
        }

        //TODO: Cambiar
        public void DoNextMovement(Vector3 position = new Vector3())
        {
            if (position == Vector3.zero)
            {
                do
                {
                    float x = UnityEngine.Random.Range(-20, 20);
                    float z = UnityEngine.Random.Range(-20, 20);
                    position = new Vector3(x, 0, z);
                }
                while (Physics.OverlapSphere(position, 4f).Where(c => c.CompareTag("Obstacle")).Count() > 0);

                //var marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                //marker.transform.localScale = new Vector3(.1f, 20f, .1f);
                //GameObject.Instantiate(marker, position, Quaternion.LookRotation(Vector3.zero));
                //GameObject.Destroy(marker, 1f);

                //Debug.Log($"Physics.CheckSphere {Actor.Name} ::: {Physics.CheckSphere(position, 4f)}");
                Debug.Log($"Physics.OverlapSphere {Actor.Name} ::: {Physics.OverlapSphere(position, 4f)}");
            }

            Movement.SetNextTarget(position);
            Movement.MoveToTarget();
        }

    }
}