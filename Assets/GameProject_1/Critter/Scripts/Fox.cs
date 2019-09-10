using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Status.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameProject_1.Status.Scripts
{
    public class Fox : MonoBehaviour
    {
        protected ActorBehaviour behaviour;

        private void Awake()
        {
            var hungryStatus = new HungryStatus(100, 90);
            var rageStatus = new RageStatus(30, 50);

            behaviour = new ActorBehaviour()
            {
                Actor = new BaseActor() { Name = "Fox" },
                StatusBaseList = new List<StatusBase>() { hungryStatus, rageStatus }
            };

            behaviour.SetEvaluateStatusAction<HungryStatus>(hungryStatus.IsHungry);
            behaviour.SetEvaluateStatusAction<RageStatus>(rageStatus.IsRage);

            //...
            behaviour.EvaluateStatus();
        }

    }
}