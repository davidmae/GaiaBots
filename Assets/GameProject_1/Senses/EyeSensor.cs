using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
using Assets.GameProject_1.Critter;
using System.Linq;
using UnityEngine;

namespace Assets.GameProject_1.Senses
{
    [RequireComponent(typeof(ConeCollider))]
    public class EyeSensor : DistanceSense
    {
        private void Awake()
        {
            Actor = GetComponentInParent<ActorBase>();
            Distance = GetComponent<ConeCollider>().Distance;
        }

        public override bool Detect(ActorBase actor, IDetectable detectable)
        {
            if (detectable == null)
                return false;

            //if (detectable is IConsumable)
            //{
            //    var statusFromConsumable = ((IConsumable)detectable).StatusModified;
            //    var sensor = Actor.Senses.Where(x => x.ExplicitStatusFromDetect == statusFromConsumable.Type).FirstOrDefault();
            //    if (this.Distance > sensor.Distance)
            //        return;
            //}

            Target = detectable;

            if (detectable is IDetectableDynamic)
            {
                var critterSrc = Actor as CritterBase;
                var critterTgt = detectable as CritterBase;
                if (critterSrc.critterData.Specie == critterTgt.critterData.Specie)
                    return false;
            }


            return base.Detect(actor, detectable);

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
                var detectable = other.GetComponent<IDetectableDynamic>();
                Detect(Actor, detectable);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other != null)
            {
                var detectable = other.GetComponent<IDetectable>();
                if (detectable == null)
                    return;

                //if (SenseBehaviour == SenseBehaviour.Agresive)
                //{
                //    if (Vector3.Distance(transform.position, detectable.GetPosition()))
                //}

                //Debug.Log($"{Actor.name} ::: Target to NULL");
                Target = null;
            }
        }
    }
}
