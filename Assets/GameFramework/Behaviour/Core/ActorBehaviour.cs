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
        public BaseActor Actor { get; set; }
        public IMovable Movement { get; set; }

        public IList<StatusBase> StatusBaseList { get; set; }

        public void SetEvaluateStatusAction<T>(Action<BaseActor> evaluate)
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
                float x = UnityEngine.Random.Range(-20, 20);
                float z = UnityEngine.Random.Range(-20, 20);
                position = new Vector3(x, 0, z);
            }

            Movement.SetNextTarget(position);
            Movement.MoveToTarget();
        }

    }
}