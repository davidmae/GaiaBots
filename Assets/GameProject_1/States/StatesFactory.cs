using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
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
        ActorBase actor;

        public StatesDictionary SelectedStates { get; private set; }

        public StatesFactory(ActorBase actor, Action<StatesFactory, StatesDictionary> states)
        {
            this.actor = actor;
            this.SelectedStates = new StatesDictionary();
            states(this, SelectedStates);
        }


        // Endpoint Actions

        public void RestoreHungry()
        {
            int value = actor.GetCurrentDetectable<IConsumable>().MinusOnePoint();

            if (value >= 0)
                actor.StatusInstances[StatusTypes.Hungry].UpdateStatus(1);

            // Debug
            //Debug.Log($"{this.name} --- {GetCurrentDetectable<IConsumable>().GetSacietyPoints()}" +
            //    $" --- saciety actor: {StatusInstances[StatusTypes.Hungry].Current}");
        }



        // Main States

        public IEnumerator IsMovingRoutine()
        {
            actor.Behaviour.StateMachine.UpdateStates(move: true);
            actor.Behaviour.Movement.MoveToPosition();

            yield return CheckIfArrivedToPosition();

            actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
            actor.Behaviour.StateMachine.UpdateStates(move: true);

            actor.Behaviour.StateMachine.Update();
        }

        public IEnumerator IsGoingtoDetectableItemRoutine()
        {
            var detectable = actor.GetCurrentDetectable<IDetectable>();

            if (detectable == null)
                yield break;

            actor.Behaviour.Movement.MoveToPosition(detectable.GetPosition());
            actor.Behaviour.StateMachine.UpdateStates(gotoItem: true);

            yield return CheckIfArrivedToPosition();

            actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.StayFront;
            actor.Behaviour.StateMachine.UpdateStates(stayfront: true);

            actor.Behaviour.StateMachine.Update();
        }

        public IEnumerator IsGoingtoFightRoutine()
        {
            var actorTarget = actor.GetCurrentDetectable<ActorBase>();

            if (actorTarget == null || actorTarget.ToString() == "null")
                yield break;

            actor.Behaviour.StateMachine.UpdateStates(gotoFight: true);
            actor.Behaviour.Movement.MoveToPosition(actorTarget.transform.position);

            yield return CheckIfArrivedToPosition();

            if (!actor.Behaviour.StateMachine.CurrentState.IsFighting)
            {
                actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Attack;
                actor.Behaviour.StateMachine.UpdateStates(fight: true);
                actor.Behaviour.StateMachine.Update();
            }
            else
                yield break;

        }

        public IEnumerator IsStayfrontRoutine()
        {
            var consumable = actor.GetCurrentDetectable<IConsumable>();

            if (consumable == null || consumable.ToString() == "null")
                yield break;

            actor.Behaviour.StateMachine.UpdateStates(stayfront: true);
            actor.Behaviour.Movement.Navigation.Stop();

            consumable.OnUpdateEntity += actor.PlusOnePointToActor(consumable);
            consumable.InvokeUpdateEntityOverTime();

            yield return CheckIfactorFinishWithConsumable(consumable);

            actor.Behaviour.Movement.Navigation.Restart();
            actor.DetectableQueue.Remove(consumable);

            consumable.OnUpdateEntity -= actor.PlusOnePointToActor(consumable);
            consumable.CancelUpdateEntityOverTime();
            consumable.DestroyEntity();

            actor.DetectableQueue.GetNextDetectable(actor);

            actor.Behaviour.StateMachine.Update();
        }

        public IEnumerator IsFightingRoutine()
        {
            var actorTarget = actor.GetCurrentDetectable<ActorBase>();

            if (actorTarget == null)
                yield break;

            actor.Behaviour.StateMachine.UpdateStates(fight: true);
            actor.Behaviour.Movement.Navigation.Stop();

            // Se resta vida al target ya que recibe primero!
            actorTarget.OnUpdateEntity += actor.MinusOnePointToActor<HealthStatus>(actorTarget);
            actorTarget.InvokeUpdateEntityOverTime();

            yield return CheckIfActorFinish(actorTarget);

            if (actor == null || actorTarget == null)
                yield break;

            actor.Behaviour.Movement.Navigation.Restart();
            actor.DetectableQueue.Remove(actorTarget);

            actorTarget.OnUpdateEntity -= actor.MinusOnePointToActor<HealthStatus>(actorTarget);
            actorTarget.CancelUpdateEntityOverTime();

            if (actor.IsTooFar(actorTarget))
                actor.Senses[0].Detect(actor, actorTarget); //<-- sets NextState inside (continue going to fight)
            else
            {
                // Terminamos de restar vida ya que el target ya no existe para golpearnos!
                actor.OnUpdateEntity -= actorTarget.MinusOnePointToActor<HealthStatus>(actor);
                actor.CancelUpdateEntityOverTime();
                actor.SetUpdateToNull();

                // Se destruye el target
                actorTarget.DestroyEntity();

                actor.Behaviour.StateMachine.NextState = StateMachine_BaseStates.Idle;
                actor.Behaviour.StateMachine.UpdateStates(move: true);
            }

            actor.Behaviour.StateMachine.Update();
        }



        // Conditionals Exit states

        private IEnumerator CheckIfArrivedToPosition()
        {
            bool done = false;
            while (!done)
            {
                var actorMovement = actor.Behaviour.Movement;

                if (actorMovement.ArrivedToPosition(
                    actor.transform.position,
                    actorMovement.Navigation.NavigatorProps.stoppingDistance))
                {
                    done = true;
                }

                yield return null;
            }
        }

        private IEnumerator CheckIfactorFinishWithConsumable(IConsumable consumable)
        {
            bool done = false;
            while (!done)
            {
                // If consumable is depleted
                if (consumable.GetCurrentPoints() <= 0)
                    done = true;

                // If actor is full
                //var status = actor.StatusInstances.GetStatusFromConsumableType(consumable);
                var status = consumable.StatusModified;

                if (actor.IsFull(status))
                    done = true;

                yield return null;
            }
        }

        private IEnumerator CheckIfActorFinish(ActorBase actorTarget)
        {
            bool done = false;
            while (!done)
            {
                if (actor.IsDeath(actorTarget))
                    done = true;

                if (actor.IsTooFar(actorTarget))
                    done = true;

                actor.transform.LookAt(actorTarget.GetPosition());

                yield return null;
            }
        }
    }
}
