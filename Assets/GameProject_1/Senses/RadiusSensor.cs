using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameProject_1.Critter;
using Assets.GameProject_1.Status;
using Assets.GameProject_1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameProject_1.Senses
{
    public class RadiusSensor : RadiusSense
    {
        private void Awake()
        {
            Actor = GetComponentInParent<ActorBase>();
        }

        private new void Update()
        {
            base.Update();

            if (StopSensor) return;

            var colliders = Physics.OverlapSphere(transform.position, Distance);

            Target = null;

            foreach (var collider in colliders)
            {
                if (ExplicitStatusFromDetect == StatusTypes.Libido)
                {
                    Target = null;

                    var actorTarget = collider.GetComponent<ActorBase>();
                    if (actorTarget != null && actorTarget != Actor)
                    {
                        if (Actor.Genre == actorTarget.Genre)
                            return;

                        var critterSrc = Actor as CritterBase;
                        var critterTgt = actorTarget as CritterBase;
                        if (critterSrc.critterData.Specie != critterTgt.critterData.Specie)
                            return;

                        if (!Actor.CanGiveBirth(actorTarget))
                            return;

                        Target = actorTarget;
                    }
                }
                else
                    Target = collider.GetComponent<IDetectable>();

                base.Detect(Actor, Target);
            }
        }

    }
}
