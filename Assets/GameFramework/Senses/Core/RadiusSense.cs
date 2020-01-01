using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Senses.Interfaces;
using Assets.GameFramework.Status.Core;
using UnityEngine;

namespace Assets.GameFramework.Senses.Core
{
    public class RadiusSense : SenseBase
    {
        //public float Distance; //Radius
        public float MaxDistance;
        public float DistanceUpdateSeconds;


        private float seconds = 0f;

        //private void FixedUpdate()
        //{
        //    if (StopSensor) return;

        //    var colliders = Physics.OverlapSphere(transform.position, Distance);

        //    //Target = null;
        //    foreach (var collider in colliders)
        //    {
        //        var detectable = collider.GetComponent<IDetectable>();

        //        if (detectable is IDetectableDynamic && ExplicitStatusFromDetect == StatusTypes.Libido)
        //        {
        //            if (Target == detectable) return;

        //            var actorTarget = detectable.GetGameObject().GetComponent<ActorBase>();
        //            if (actorTarget == null) return;
        //            if (Actor.Genre == actorTarget.Genre)
        //                return;
        //        }

        //        Detect(Actor, detectable);
        //    }
        //}


        protected void Update()
        {
            if (StopSensor)
                return;

            if (ExplicitStatusFromDetect == StatusTypes.Undefined) 
                return;

            StatusBase status;
            if (Actor.StatusInstances.TryGetValue(ExplicitStatusFromDetect, out status) && status.LimitReached())
                return;

            if (seconds >= DistanceUpdateSeconds)
            {
                status?.UpdateStatus(this);
                seconds = 0f;
            }
            else
                seconds += Time.deltaTime;

        }

        private void OnDrawGizmosSelected()
        {
            if (Distance > MaxDistance) Distance = MaxDistance;

            #region Debugging

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Distance);

            #endregion
        }

    }

    
}
