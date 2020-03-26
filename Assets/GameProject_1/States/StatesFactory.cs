using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameProject_1.Status;
using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Assets.GameProject_1.Critter;
using Assets.GameProject_1.Utils;

namespace Assets.GameProject_1.States
{
    public class StatesFactory
    {
        ActorBase Actor;

        float currentTime;
        Vector3 lastPosition;

        public StatesDictionary SelectedStates { get; private set; }

        public StatesFactory(ActorBase actor, Action<StatesFactory, StatesDictionary> states)
        {
            this.Actor = actor;
            this.SelectedStates = new StatesDictionary();
            states(this, SelectedStates);
        }


        // Main States

        public IEnumerator IsMovingRoutine()
        {
            Actor.Behaviour.StateMachine.UpdateStates(move: true);
            Actor.Behaviour.Movement.MoveToPosition();

            yield return CheckIfArrivedToPosition();

            Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
            Actor.Behaviour.StateMachine.UpdateStates(move: true);

            Actor.Behaviour.StateMachine.Update();

            yield return null;
        }

        public IEnumerator IsGoingtoDetectableItemRoutine()
        {
            var detectable = Actor.GetCurrentDetectable<IDetectable>();

            if (detectable == null || detectable.ToString() == "null")
                yield break;

            Actor.Behaviour.StateMachine.UpdateStates(gotoItem: true);
            Actor.Behaviour.Movement.MoveToPosition(detectable.GetPosition());

            yield return CheckIfArrivedToPositionConsumable(detectable);

            if (detectable == null || detectable.ToString() == "null" ||
                Actor.Senses.AnySenseWithTarget())
            {
                Actor.DetectableQueue.Remove(detectable);

                var nextDetectable = Actor.DetectableQueue.GetNextDetectable(Actor);
                if (nextDetectable == null)
                {
                    Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                    Actor.Behaviour.StateMachine.UpdateStates(move: true);
                    //Actor.Behaviour.StateMachine.Update();
                }
                else
                {
                    if (!Actor.Senses[0].Detect(Actor, nextDetectable))
                        Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                }
            }
            else
            {
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.StayFront;
                Actor.Behaviour.StateMachine.UpdateStates(stayfront: true);
            }
            
            Actor.Behaviour.StateMachine.Update();

            yield return null;
        }

        public IEnumerator IsGoingtoFightRoutine()
        {
            var actorTarget = (ActorBase)Actor.CurrentTarget;

            //var actorTarget = (ActorBase)actor.DetectableQueue.First();

            if (actorTarget == null || actorTarget.ToString() == "null")
                yield break;

            Actor.Behaviour.StateMachine.UpdateStates(gotoFight: true);
            Actor.Behaviour.Movement.MoveToPosition(actorTarget.transform.position);

            yield return CheckIfArrivedToPositionToFight(actorTarget);

            //var speed = ((CritterBase)Actor).critterData.Speed;
            //Actor.Behaviour.Movement.Navigation.SetSpeed(speed);

            if (actorTarget == null || actorTarget.ToString() == "null" ||
                Actor.Senses.AnySenseWithTarget<DistanceSense>())
            {
                Actor.CurrentTarget = null;
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
            }
            else
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Attack;

            Actor.Behaviour.StateMachine.Update();

            yield return null;
        }

        public IEnumerator IsGoingtoLoveRoutine()
        {
            var actorTarget = (ActorBase)Actor.CurrentTarget;

            //var actorTarget = (ActorBase)actor.DetectableQueue.First();

            if (actorTarget == null || actorTarget.ToString() == "null")
                yield break;

            Actor.Behaviour.StateMachine.UpdateStates(gotoLove: true);
            Actor.Behaviour.Movement.MoveToPosition(actorTarget.transform.position);

            yield return CheckIfArrivedToPositionConsumable(actorTarget);

            if (actorTarget == null || actorTarget.ToString() == "null" ||
                Actor.Senses.AnySenseWithTarget<DistanceSense>())
            {
                Actor.CurrentTarget = null;
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
            }
            else
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.StayFrontEntity;

            Actor.Behaviour.StateMachine.Update();

            yield return null;
        }

        public IEnumerator IsStayFrontEntityRoutine()
        {
            var actorTarget = (ActorBase)Actor.CurrentTarget;

            if (actorTarget == null)
                yield break;

            Actor.Behaviour.StateMachine.UpdateStates(stayLoving: true);
            Actor.Behaviour.Movement.Navigation.Stop();
            Actor.Senses.StopSensorsWithTarget(actorTarget);

            //todo
            actorTarget.Behaviour.Movement.Navigation.Stop();

            yield return new WaitForSeconds(5);

            if (Actor == null || actorTarget == null)
                yield break;
            
            //todo
            actorTarget.Behaviour.Movement.Navigation.Restart();

            Actor.StatusInstances[StatusTypes.Libido].Current = 0f;

            Actor.Behaviour.Movement.Navigation.Restart();
            Actor.Senses.RestartSensorsWithTarget(actorTarget);

            //Actor.DetectableQueue.Remove(actorTarget);

            //todo
            var sensor = Actor.Senses.Where(x => x.ExplicitStatusFromDetect == StatusTypes.Libido).FirstOrDefault();
            sensor.Distance = 0f;
            sensor.Target = null;
            sensor.TargetName = "";

            Actor.GiveBirthBabyCritter(actorTarget);

            Actor.CurrentTarget = null;
            Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
            Actor.Behaviour.StateMachine.Update();

            yield return null;
        }

        public IEnumerator IsStayFrontConsumableRoutine()
        {
            var consumable = Actor.GetCurrentDetectable<IConsumable>();

            if (consumable == null || consumable.ToString() == "null")
                yield break;

            Actor.Behaviour.StateMachine.UpdateStates(stayfront: true);
            Actor.Behaviour.Movement.Navigation.Stop();
            Actor.Senses.StopSensorsWithTarget(consumable);

            //Se obtiene el sensor que queremos modificar (en este momento los sentidos con radio de alcance)
            var senseFrom = Actor.Senses.OfType<RadiusSense>().Where(x => x.TargetName == consumable.GetGameObject().name).FirstOrDefault();

            consumable.OnUpdateEntity += Actor.IncrementStatus(consumable, senseFrom);
            consumable.InvokeUpdateEntityOverTime(1, 1);

            yield return CheckIfActorFinishConsumable(consumable);

            Actor.DetectableQueue.Remove(consumable);
            Actor.Behaviour.Movement.Navigation.Restart();
            Actor.Senses.RestartSensorsWithTarget(consumable);

            consumable.OnUpdateEntity -= Actor.IncrementStatus(consumable, senseFrom);
            consumable.CancelUpdateEntityOverTime();
            consumable.OnUpdateEntityToNull();

            if (consumable.GetCurrentDurability() <= 0)
            {
                consumable.DestroyEntity();
            }

            var nextDetectable = Actor.DetectableQueue.GetNextDetectable(Actor);
            if (nextDetectable == null)
            {
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                Actor.Behaviour.StateMachine.UpdateStates(move: true);
                //Actor.Behaviour.StateMachine.Update();
            }
            else
            {
                if (!Actor.Senses[0].Detect(Actor, nextDetectable))
                    Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;

                //Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.GoingToItem;
                //Actor.Behaviour.StateMachine.UpdateStates(gotoItem: true);
            }

            Actor.Behaviour.StateMachine.Update();

            yield return null;
        }

        public IEnumerator IsFightingRoutine()
        {
            var actorTarget = (ActorBase)Actor.CurrentTarget;

            if (actorTarget == null)
                yield break;

            Actor.Behaviour.StateMachine.UpdateStates(fight: true);
            Actor.Behaviour.Movement.Navigation.Stop();

            //actorTarget.SetUpdateToNull();

            // Se resta vida al target ya que recibe primero!
            actorTarget.OnUpdateEntity += Actor.MinusOnePointToActor<HealthStatus>(actorTarget);

            if (Actor.Behaviour.HostilityBehaviour?.Value > 0)
            {
                Actor.OnUpdateEntity += () => Actor.StatusInstances[StatusTypes.Hungry].UpdateStatus(1);
                Actor.InvokeUpdateEntityOverTime(1, 1);
            }

            actorTarget.InvokeUpdateEntityOverTime(1, 1);

            yield return CheckIfActorFinish(actorTarget);

            if (Actor == null || actorTarget == null)
                yield break;

            Actor.Behaviour.Movement.Navigation.Restart();
            Actor.DetectableQueue.Remove(actorTarget);

            actorTarget.OnUpdateEntity -= Actor.MinusOnePointToActor<HealthStatus>(actorTarget);
            actorTarget.CancelUpdateEntityOverTime();
            actorTarget.OnUpdateEntityToNull();

            // Terminamos de restar vida ya que el target ya no existe para golpearnos!
            Actor.OnUpdateEntity -= actorTarget.MinusOnePointToActor<HealthStatus>(Actor);
            Actor.CancelUpdateEntityOverTime();
            Actor.OnUpdateEntityToNull();

            //Actor.CurrentTarget = null;

            if (Actor.Senses.OfType<DistanceSense>().Count(x => x.SenseBehaviour == SenseBehaviour.Agresive && x.Target == null) > 0)
            {
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.GoingToFight;
                Actor.Behaviour.StateMachine.Update();
            }
            else if (Actor.IsDeath(actorTarget))
            {
                actorTarget.DestroyEntity();

                Actor.CurrentTarget = null;
                Actor.Senses.SetSensorsTargetToNull<DistanceSense>(SenseBehaviour.Agresive);
                Actor.Behaviour.Movement.Navigation.SetSpeed(((CritterBase)Actor).critterData.Speed);
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                Actor.Behaviour.StateMachine.Update();
            }

            yield return null;
        }

        public IEnumerator IsStandToEntityRoutine()
        {
            var actorTarget = (ActorBase)Actor.CurrentTarget;

            if (actorTarget == null)
                yield break;

            var hostilityAI = Actor.Behaviour.HostilityBehaviour;

            Actor.Behaviour.StateMachine.UpdateStates(standto: true);
            Actor.Behaviour.Movement.Navigation.SetSpeed(0);

            yield return CheckIfStandToFinish();

            var distance = Vector3.Distance(Actor.transform.position, Actor.CurrentTarget.GetPosition());
            Debug.Log("Distancia: " + distance);

            Actor.Behaviour.StateMachine.UpdateStates(standto: false);

            var speed = ((CritterBase)Actor).critterData.Speed;

            if (distance > hostilityAI.SightDistance(hostilityAI.Value, Actor.CurrentSensorDistance))
            {
                Actor.CurrentTarget = null;
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
            }
            else
            {
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.GoingToFight;
                speed = hostilityAI.SpeedToFight(hostilityAI.Value, speed);
            }

            Actor.Behaviour.Movement.Navigation.SetSpeed(speed);
            Debug.Log("speed: " + Actor.Behaviour.Movement.Navigation.GetSpeed());

            Actor.Behaviour.StateMachine.Update();

            yield return null;
        }


        // Conditionals Exit states

        private IEnumerator CheckIfStandToFinish()
        {
            var hostilityAI = Actor.Behaviour.HostilityBehaviour;
            var secondsFinish = hostilityAI.SecondsStandTo(hostilityAI.Value);
            Debug.Log($"waiting {secondsFinish} seconds");

            var currentTime = 0f;
            var totalTime = 0f;
            var done = false;
            while (!done)
            {
                if (currentTime > 0.5f)
                {
                    var distance = Vector3.Distance(Actor.transform.position, Actor.CurrentTarget.GetPosition());
                    if (hostilityAI.OutOfSight(hostilityAI.Value, distance, Actor.CurrentSensorDistance))
                        done = true;

                    if (totalTime >= secondsFinish)
                        done = true;

                    currentTime = 0f;
                }
                else
                    currentTime += Time.deltaTime;

                totalTime += Time.deltaTime;

                yield return null;
            }
        }

        private IEnumerator CheckIfArrivedToPosition()
        {
            bool done = false;
            while (!done)
            {
                var actorMovement = Actor.Behaviour.Movement;

                //if (actorMovement.ArrivedToPosition(Actor.transform.position, actorMovement.Navigation.NavigatorProps.stoppingDistance))
                if (actorMovement.ArrivedToPosition(Actor.transform.position, actorMovement.CritterData.StopingDistance))
                    done = true;
                
                //Avoid blocked movement
                if (currentTime > 3f)
                {
                    currentTime = 0f;
                    if (Vector3.Distance(Actor.transform.position, lastPosition) < 1f)
                        done = true;
                }

                currentTime += Time.deltaTime;
                lastPosition = Actor.transform.position;

                yield return null;
            }
        }

        private IEnumerator CheckIfArrivedToPositionConsumable(IDetectable detectable)
        {
            bool done = false;
            while (!done)
            {
                if (detectable == null || detectable.ToString() == "null")
                    yield break;

                var actorMovement = Actor.Behaviour.Movement;

                var actorBounds = Actor.GetComponent<Collider>().bounds;
                var targetBounds = detectable.GetGameObject().GetComponent<Collider>().bounds;

                //Success = true
                if (actorBounds.Intersects(targetBounds))
                    done = true;

                //Success = false
                if (Actor.Senses.AnySenseWithTarget())
                    done = true;

                Actor.transform.LookAt(detectable.GetPosition());
                actorMovement.MoveToPosition(detectable.GetPosition());

                yield return null;
            }
        }

        private IEnumerator CheckIfArrivedToPositionToFight(IDetectable detectable)
        {
            bool done = false;
            while (!done)
            {
                if (detectable == null || detectable.ToString() == "null")
                    yield break;

                var actorMovement = Actor.Behaviour.Movement;

                var actorBounds = Actor.GetComponent<Collider>().bounds;
                var targetBounds = detectable.GetGameObject().GetComponent<Collider>().bounds;

                //Success = true
                if (actorBounds.Intersects(targetBounds))
                    done = true;
                
                //Success = true
                var distance = Vector3.Distance(Actor.transform.position, detectable.GetPosition());
                if (Actor.Senses.IsInAttackRange(distance))
                    done = true;

                //Success = false
                if (Actor.Senses.AnySenseWithTarget<DistanceSense>())
                    done = true;

                Actor.transform.LookAt(detectable.GetPosition());
                actorMovement.MoveToPosition(detectable.GetPosition());

                yield return null;
            }
        }

        private IEnumerator CheckIfActorFinishConsumable(IConsumable consumable)
        {
            bool done = false;
            while (!done)
            {
                //Success = true
                if (consumable.GetCurrentDurability() <= 0)
                    done = true;

                //Success = true
                var status = consumable.StatusModified;
                if (Actor.IsFull(status))
                    done = true;

                var actorBounds = Actor.GetComponent<Collider>().bounds;
                var targetBounds = consumable.GetGameObject().GetComponent<Collider>().bounds;

                //Success = false
                if (!actorBounds.Intersects(targetBounds))
                    done = true;

                //Success = false
                if (Actor.Senses.Count(x => x.Target == consumable) <= 0)
                    done = true;

                //if (Actor.Senses.Count(x => x.ExplicitStatusFromDetect == StatusTypes.Hungry && x.Target == null) > 0)
                //    done = true;

                yield return null;
            }
        }

        private IEnumerator CheckIfActorFinish(ActorBase actorTarget)
        {
            bool done = false;
            while (!done)
            {
                //Success = true
                if (Actor.IsDeath(actorTarget))
                    done = true;

                //Success = false
                if (Actor.Senses.OfType<DistanceSense>().Count(x => x.SenseBehaviour == SenseBehaviour.Agresive && x.Target == null) > 0)
                    done = true;

                Actor.transform.LookAt(actorTarget.GetPosition());

                yield return null;
            }
        }
    }
}
