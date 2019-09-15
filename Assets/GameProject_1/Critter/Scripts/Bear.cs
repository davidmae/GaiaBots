using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameProject_1.Status;
using Assets.GameProject_1.Status.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameProject_1.Critter.Scripts
{
    public class Bear : CritterBase
    {
        private void Awake()
        {
            var hungryStatus = new HungryStatus(75, 100);
            var rageStatus = new RageStatus(75, 100);
            var navigator = GetComponent<NavMeshAgent>();


            CritterData.Name = "Bear";


            _behaviour = new ActorBehaviour()
            {
                Actor = new ActorBase() { Name = "Bear" },
                Movement = new MovableAI() { Navigator = navigator },
                StatusBaseList = new List<StatusBase>() { hungryStatus, rageStatus }
            };

            //_behaviour.SetEvaluateStatusAction<HungryStatus>(hungryStatus.IsHungry);
            //_behaviour.SetEvaluateStatusAction<RageStatus>(rageStatus.IsRage);

            ////...
            //_behaviour.EvaluateStatus();
        }
    }
}
