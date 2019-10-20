using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Interfaces;
using Assets.GameFramework.Status.Core;
using UnityEngine;

namespace Assets.GameFramework.Senses.Core
{
    public enum SenseDetectionType
    {
        DetectAll = 0,
        DetectRelative = 1
    }

    public enum SenseBehaviour
    {
        Undefined = -1,
        Agresive = 0,
        Pacific = 1
    }

    public static class SenseBaseExtensions
    {        
        public static SenseBase SenseMaxDistance(this List<SenseBase> senses, SenseBehaviour behaviour = SenseBehaviour.Undefined)
        {
            var senseWithMaxDistance = senses;

            if (behaviour != SenseBehaviour.Undefined)
                senseWithMaxDistance = senseWithMaxDistance.Where(sense => sense.SenseBehaviour == behaviour).ToList();

            return senseWithMaxDistance.Aggregate((s1, s2) => s1.Distance > s2.Distance ? s1 : s2);
        }
        public static SenseBase SenseMaxDistance<TSense>(this List<SenseBase> senses, SenseBehaviour behaviour = SenseBehaviour.Undefined) where TSense : SenseBase
        {
            var senseWithMaxDistance = senses.OfType<TSense>();

            if (behaviour != SenseBehaviour.Undefined)
                senseWithMaxDistance = senseWithMaxDistance.Where(sense => sense.SenseBehaviour == behaviour).ToList();

            return senseWithMaxDistance.Aggregate((s1, s2) => s1.Distance > s2.Distance ? s1 : s2);
        }
        public static void SetSensorsTargetToNull<TSense>(this List<SenseBase> senses, SenseBehaviour behaviour = SenseBehaviour.Undefined) where TSense : SenseBase
        {
            senses.OfType<TSense>().Where(x => x.SenseBehaviour == behaviour).Select(x => { x.Target = null; return x; }).ToList();
        }
        public static bool AnySenseWithTarget<TSense>(this List<SenseBase> senses, SenseBehaviour behaviour = SenseBehaviour.Undefined) where TSense : SenseBase
        {
            return senses.OfType<TSense>().Count(x => x.Target != null) == 0;
        }
        public static bool AnySenseWithTarget(this List<SenseBase> senses, SenseBehaviour behaviour = SenseBehaviour.Undefined)
        {
            return senses.Count(x => x.Target != null) == 0;
        }
    }

    public abstract class SenseBase : MonoBehaviour, ISense
    {
        protected ActorBase Actor;
        public IDetectable Target;

        public float Distance;
        public SenseDetectionType DetectionType;
        public StatusTypes ExplicitStatusFromDetect;
        public SenseBehaviour SenseBehaviour;

        public virtual void Detect(ActorBase actor, IDetectable detectable)
        {
            if (detectable == null)
                return;

            if (actor == null)
                return;

            if (actor.name == detectable.GetGameObject().name)
                return;

            Target = detectable;

            if (actor.Behaviour.StateMachine.CurrentState.IsStayFront)
                return;

            if (actor.Behaviour.StateMachine.CurrentState.IsFighting)
                return;

            if (detectable is IConsumable)
            {
                var statusFromConsumable = ((IConsumable)detectable).StatusModified;

                if (statusFromConsumable == null)
                    return;

                if (actor.IsFull(statusFromConsumable))
                    return;

                if (DetectionType == SenseDetectionType.DetectRelative)
                {
                    if (statusFromConsumable.Type != ExplicitStatusFromDetect)
                        return;
                }
                
            }

            detectable.Detect(actor);
        }

    }


}
