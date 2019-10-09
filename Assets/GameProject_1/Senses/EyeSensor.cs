using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
using Assets.GameProject_1.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameProject_1.Senses
{
    [RequireComponent(typeof(ConeCollider))]
    public class EyeSensor : DistanceSense
    {
        private IDetectable target;

        private void Awake()
        {
            Actor = GetComponentInParent<ActorBase>();
            Distance = GetComponent<ConeCollider>().Distance;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                var detectable = other.GetComponent<IDetectable>();
                Detect(Actor, detectable);
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other != null)
            {
                if (Actor.Behaviour.StateMachine.CurrentState.IsGoingToFight)
                {
                    var detectable = other.GetComponent<IDetectableDynamic>();
                    Detect(Actor, detectable);
                }
            }
        }
    }
}
