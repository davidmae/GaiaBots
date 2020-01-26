using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Interfaces;
using UnityEngine;

namespace Assets.GameFramework.Senses.Core
{
    public class DistanceSense : SenseBase
    {
        public override bool Detect(ActorBase actor, IDetectable detectable)
        {
            if (detectable == null)
                return false;

            if (detectable is IConsumable)
            {
                var statusFromConsumable = ((IConsumable)detectable).StatusModified;
                var sensor = Actor.Senses.Where(x => x.ExplicitStatusFromDetect == statusFromConsumable.Type).FirstOrDefault();
                if (this.Distance > sensor.Distance)
                    return false;
            }

            if (detectable is IDetectableDynamic &&
                SenseBehaviour == SenseBehaviour.Agresive &&
                Vector3.Distance(transform.position, detectable.GetPosition()) > Distance)
            {
                return false;
            }

            return base.Detect(actor, detectable);
        }
    }
}
