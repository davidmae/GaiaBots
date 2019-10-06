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

namespace Assets.GameProject_1.Senses
{
    public class Sensor : SensesBase
    {
        public override void Detect(ActorBase actor, IDetectable detectable)
        {
            if (detectable == null)
                return;

            if (detectable is IConsumable)
            {
                var status = actor.StatusInstances.GetStatusFromConsumableType((IConsumable)detectable);

                if (actor.IsFull(status))
                    return;
            }
                
            actor.transform.LookAt(detectable.GetPosition());
            detectable.Detect(actor);
        }
    }
}
