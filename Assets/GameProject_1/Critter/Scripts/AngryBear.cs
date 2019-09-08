using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Status.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameProject_1.Status.Scripts
{
    public class AngryBear : MonoBehaviour
    {
        protected BaseActor baseActor;
        protected ActorBehaviour behaviour;

        private void Awake()
        {
            baseActor = new BaseActor() { Name = "AngryBear" };

            var hungryStatus = new HungryStatus(75, 100);
            var rageStatus = new RageStatus(75, 100);

            behaviour = new ActorBehaviour()
            {
                Actor = baseActor,
                StatusBaseList = new List<StatusBase>() { hungryStatus, rageStatus }
            };

            behaviour.SetEvaluateStatusAction<HungryStatus>(hungryStatus.IsHungry);
            behaviour.SetEvaluateStatusAction<RageStatus>(rageStatus.IsRage);

            //...
            behaviour.EvaluateStatus();
        }

    }
}