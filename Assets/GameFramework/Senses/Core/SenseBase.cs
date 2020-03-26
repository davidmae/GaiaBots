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
        public static void StopSensorsWithTarget(this List<SenseBase> senses, IGFrameworkEntityBase target) => senses.Where(x => x.Target == target).ToList().ForEach(x => x.Stop());
        public static void RestartSensorsWithTarget(this List<SenseBase> senses, IGFrameworkEntityBase target) => senses.Where(x => x.Target == target).ToList().ForEach(x => x.Restart());
        public static bool IsInAttackRange(this List<SenseBase> senses, float distance)
        {
            var attackSensor = senses.OfType<DistanceSense>().Where(x => x.SenseBehaviour == SenseBehaviour.Agresive && x.Target != null).FirstOrDefault();
            if (attackSensor == null) return false;
            //Debug.Log($"distance {distance} --- attackSensor.Distance {attackSensor.Distance}");
            return distance < attackSensor.Distance;
        }
        public static SenseBase GetSenseWithTarget(this List<SenseBase> senses, IDetectable target) => senses.Where(x => x.Target == target).FirstOrDefault();
    }

    public abstract class SenseBase : MonoBehaviour, ISense
    {
        protected ActorBase Actor;
        public IDetectable Target;

        public float Distance;
        public bool StopSensor = false;

        public SenseDetectionType DetectionType;
        public StatusTypes ExplicitStatusFromDetect;
        public SenseBehaviour SenseBehaviour;

        public string TargetName;

        public virtual bool Detect(ActorBase actor, IDetectable detectable)
        {
            Target = null;
            TargetName = "";

            if (StopSensor)
                return false;

            if (detectable == null)
                return false;

            if (actor == null)
                return false;

            if (actor.gameObject == detectable.GetGameObject())
                return false;

            if (actor.Behaviour.StateMachine.CurrentState.IsStayFront)
                return false;

            if (actor.Behaviour.StateMachine.CurrentState.IsFighting)
                return false;

            if (detectable is IConsumable)
            {
                var statusFromConsumable = ((IConsumable)detectable).StatusModified;

                if (statusFromConsumable == null)
                    return false;

                if (actor.IsFull(statusFromConsumable))
                    return false;

                if (DetectionType == SenseDetectionType.DetectRelative)
                {
                    if (statusFromConsumable.Type != ExplicitStatusFromDetect)
                        return false;
                }
                
            }

            if (detectable is IDetectableDynamic)
            {
                if (DetectionType == SenseDetectionType.DetectRelative)
                {
                    if (ExplicitStatusFromDetect != StatusTypes.Undefined)
                        return false;
                }
            }

            Target = detectable;
            TargetName = detectable.GetGameObject().name;

            Actor.CurrentSensorDistance = Distance;

            return detectable.Detect(actor, this);
        }

        public void Stop() => StopSensor = true;
        public void Restart() => StopSensor = false;

    }


}
