using Assets.GameFramework.Status.Core;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.GameFramework.Actor.Core;

namespace Assets.GameFramework.Behaviour.Core
{
    public class ActorBehaviour
    {
        public BaseActor Actor { get; set; }
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
    }
}