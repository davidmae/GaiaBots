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

namespace Assets.GameProject_1.States
{
    public class StatesFactory
    {
        ActorBase Actor;

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
                    Actor.Behaviour.StateMachine.Update();
                }
                else
                {
                    Actor.Senses[0].Detect(Actor, nextDetectable);
                }
            }
            else
            {
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.StayFront;
                Actor.Behaviour.StateMachine.UpdateStates(stayfront: true);
                Actor.Behaviour.StateMachine.Update();
            }

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

            if (actorTarget == null || actorTarget.ToString() == "null" ||
                Actor.Senses.AnySenseWithTarget<DistanceSense>())
            {
                Actor.CurrentTarget = null;
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                Actor.Behaviour.StateMachine.UpdateStates(move: true);
            }
            else
            {
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Attack;
                Actor.Behaviour.StateMachine.UpdateStates(fight: true);
            }

            Actor.Behaviour.StateMachine.Update();

            yield return null;
        }

        public IEnumerator IsStayfrontRoutine()
        {
            var consumable = Actor.GetCurrentDetectable<IConsumable>();

            if (consumable == null || consumable.ToString() == "null")
                yield break;

            Actor.Behaviour.StateMachine.UpdateStates(stayfront: true);
            Actor.Behaviour.Movement.Navigation.Stop();

            consumable.OnUpdateEntity += Actor.PlusOnePointToActor(consumable);
            consumable.InvokeUpdateEntityOverTime(1, 1);

            yield return CheckIfActorFinishConsumable(consumable);

            Actor.Behaviour.Movement.Navigation.Restart();
            Actor.DetectableQueue.Remove(consumable);

            consumable.OnUpdateEntity -= Actor.PlusOnePointToActor(consumable);
            consumable.CancelUpdateEntityOverTime();
            consumable.SetUpdateToNull();

            if (consumable.GetCurrentPoints() <= 0)
            {
                consumable.DestroyEntity();
            }

            var nextDetectable = Actor.DetectableQueue.GetNextDetectable(Actor);
            if (nextDetectable == null)
            {
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                Actor.Behaviour.StateMachine.UpdateStates(move: true);
                Actor.Behaviour.StateMachine.Update();
            }
            else
            {
                Actor.Senses[0].Detect(Actor, nextDetectable);
                //Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.GoingToItem;
                //Actor.Behaviour.StateMachine.UpdateStates(gotoItem: true);
            }

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
            actorTarget.InvokeUpdateEntityOverTime(1, 1);

            yield return CheckIfActorFinish(actorTarget);

            if (Actor == null || actorTarget == null)
                yield break;

            Actor.Behaviour.Movement.Navigation.Restart();
            Actor.DetectableQueue.Remove(actorTarget);

            actorTarget.OnUpdateEntity -= Actor.MinusOnePointToActor<HealthStatus>(actorTarget);
            actorTarget.CancelUpdateEntityOverTime();
            actorTarget.SetUpdateToNull();

            // Terminamos de restar vida ya que el target ya no existe para golpearnos!
            Actor.OnUpdateEntity -= actorTarget.MinusOnePointToActor<HealthStatus>(Actor);
            Actor.CancelUpdateEntityOverTime();
            Actor.SetUpdateToNull();

            Actor.CurrentTarget = null;

            if (Actor.Senses.OfType<DistanceSense>().Count(x => x.SenseBehaviour == SenseBehaviour.Agresive && x.Target == null) > 0)
            {
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.GoingToFight;
                Actor.Behaviour.StateMachine.UpdateStates(gotoFight: true);
                Actor.Behaviour.StateMachine.Update();
            }
            else if (Actor.IsDeath(actorTarget))
            {
                actorTarget.DestroyEntity();

                Actor.Senses.SetSensorsTargetToNull<DistanceSense>(SenseBehaviour.Agresive);
                Actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                Actor.Behaviour.StateMachine.UpdateStates(move: true);
                Actor.Behaviour.StateMachine.Update();
            }

            yield return null;
        }



        // Conditionals Exit states

        private IEnumerator CheckIfArrivedToPosition()
        {
            bool done = false;
            while (!done)
            {
                var actorMovement = Actor.Behaviour.Movement;

                if (actorMovement.ArrivedToPosition(Actor.transform.position, actorMovement.Navigation.NavigatorProps.stoppingDistance))
                    done = true;

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
                if (Actor.Senses.OfType<DistanceSense>().Count(x => x.SenseBehaviour == SenseBehaviour.Agresive && x.Target != null) > 0)
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
                if (consumable.GetCurrentPoints() <= 0)
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
                if (Actor.Senses.Count(x => x.SenseBehaviour == SenseBehaviour.Pacific && x.Target == null) > 0)
                    done = true;

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
