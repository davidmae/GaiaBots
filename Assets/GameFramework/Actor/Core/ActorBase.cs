﻿using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.GameFramework.Senses.Interfaces;

namespace Assets.GameFramework.Actor.Core
{
    public class PriorityQueue<T> : List<T> where T : IDetectable
    {
        public void GetNextDetectable(ActorBase actor)
        {
            if (this.Count > 0)
            {
                if (actor.IsFull(StatusTypes.Hungry))
                {
                    //TODO: Eliminar todos los items consumables que modifiquen el hambre
                    // habrá que distinguir según status modificable
                    this.Clear();
                }
                else
                {
                    var detectable = actor.DetectableQueue[0];
                    actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.GoingToItem;
                    actor.Behaviour.StateMachine.UpdateStates(gotoItem: true);

                    //TODO: APAÑAR
                    actor.Senses[0].Detect(actor, detectable);

                    return;
                }
            }

            actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
            actor.Behaviour.StateMachine.UpdateStates(move: true);
        }
    }

    public class ActorBase : GFrameworkEntityBase, IDetectableDynamic
    {
        public ActorBehaviour Behaviour { get; set; }
        public IDictionary<StatusTypes, StatusBase> StatusInstances { get; set; }
        public PriorityQueue<IDetectable> DetectableQueue { get; set; }
        public List<SenseBase> Senses { get; set; }

        public float attackDistance;

        [Header("Debugging fields")]

        // -------- Debugging on inspector --------
        public States CurrentState;
        public List<StatusBase> ListStatus;
        // ----------------------------------------

        public virtual void Detect(ActorBase originalActor)
        {
            if (this == null || this.ToString() == "null")
                return;

            if (originalActor == this) //<<-- En ocasiones se detecta el mismo 
                return;

            originalActor.transform.LookAt(transform.position);

            var detectDistance = Vector3.Distance(originalActor.transform.position, transform.position);

            var visionDistance = originalActor.GetComponentInChildren<DistanceSense>().Distance;
            if (visionDistance < detectDistance)
            {
                originalActor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                originalActor.Behaviour.StateMachine.Update();
                return;
            }

            if (originalActor.attackDistance > detectDistance)
            {
                originalActor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Attack;
                originalActor.Behaviour.StateMachine.Update();
                return;
            }

            if (!originalActor.DetectableQueue.Contains(this))
                originalActor.DetectableQueue.Add(this);

            originalActor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.GoingToFight;
            originalActor.Behaviour.StateMachine.Update();

            //-----------------------------For debugging------------------------------
            //var marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            //marker.transform.localScale = new Vector3(.1f, 20f, .1f);
            //GameObject.Instantiate(marker, transform.position, Quaternion.LookRotation(Vector3.zero));
            //GameObject.Destroy(marker, 1f);

            //Debug.Log($"{name} has detected to {currentActor.name}");
        }

        public Vector3 GetPosition() => transform.position;

        public virtual T GetCurrentDetectable<T>() where T : IDetectable
        {
            foreach (var detectable in DetectableQueue)
            {
                try
                {
                    var res = (T)detectable;
                    return res;
                }
                catch (Exception ex)
                { }
            }

            return default(T);
        }

        public virtual bool IsFull(StatusBase status)
        {
            if (status == null) return true;
            status = StatusInstances.Values.FirstOrDefault(s => s.GetType() == status.GetType());
            return status.Current >= status.MaxValue;
        }
        public virtual bool IsFull(StatusTypes statusType)
        {
            var status = this.StatusInstances[statusType];
            return status.Current >= status.MaxValue;
        }
        public virtual bool IsFull()
        {
            bool isFull = true;

            foreach (var status in StatusInstances.Values)
            {
                if (status.Current >= status.MaxValue)
                    continue;

                isFull = false;
                break;
            }

            return isFull;
        }

        public virtual bool IsTooFar(ActorBase actorTarget)
        {
            var detectDistance = Vector3.Distance(transform.position, actorTarget.transform.position);
            return attackDistance < detectDistance;
        }
        public virtual bool IsDeath(ActorBase actorTarget)
        {
            var status = actorTarget.StatusInstances[StatusTypes.Health];
            return status.Current <= 0;
        }


        public virtual Action PlusOnePointToActor(IConsumable consumable)
        {
            return () =>
            {
                int currValue = consumable.MinusOnePoint();

                if (currValue >= 0)
                {
                    var status = StatusInstances.Values.FirstOrDefault(s => s.GetType() == consumable.StatusModified.GetType());
                    status.UpdateStatus(1);
                }
            };
        }

        public virtual Action PlusOnePointToActor(StatusTypes statusType, IConsumable consumable)
        {
            return () =>
            {
                int currValue = consumable.MinusOnePoint();

                if (currValue >= 0)
                    StatusInstances[statusType].UpdateStatus(1);
            };
        }
        public virtual Action PlusOnePointToActor<TActorStatus>(IConsumable consumable)
            where TActorStatus : StatusBase
        {
            return () =>
            {
                int currValue = consumable.MinusOnePoint();

                if (currValue >= 0)
                {
                    StatusInstances.Values
                        .FirstOrDefault(s => s.GetType() == typeof(TActorStatus))
                        .UpdateStatus(1);
                }
            };
        }

        public virtual Action MinusOnePointToActor<TActorStatus>(IDetectableDynamic target)
            where TActorStatus : StatusBase
        {
            return () =>
            {
                var targetActor = (ActorBase)target;
                int currValue = targetActor.StatusInstances.Values.FirstOrDefault(s => s.GetType() == typeof(TActorStatus)).Current;

                if (currValue >= 0)
                {
                    targetActor
                        .StatusInstances.Values
                        .FirstOrDefault(s => s.GetType() == typeof(TActorStatus))
                        .UpdateStatus(-1);
                }
            };
        }

        
    }
}
