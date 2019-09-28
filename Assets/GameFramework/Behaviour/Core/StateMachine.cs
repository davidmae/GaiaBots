using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameFramework.Behaviour.Core
{
    public class StateMachine
    {
        public ActorBase Actor { get; set; }
        public States CurrentState { get; set; }

        //public Func<IEnumerator> NextAction { get; set; }


        public StateMachine(ActorBase actor)
        {
            Actor = actor;
            CurrentState = new States();
        }

        public void Detect(Common.IDetectable detectable)
        {
            if (detectable != null && !IfActorIsFull(StatusTypes.Hungry))
                detectable.Detect(Actor);
        }

        public void ExecuteAction(Func<IEnumerator> action) // para más tipos de datos ¿? TODO
        {
            Actor.StartCoroutine(action());
        }

        public void ExecuteAction(Func<float, IEnumerator> action, float data) // para más tipos de datos ¿? TODO
        {
            Actor.StartCoroutine(action(data));
        }

        public void UpdateStates(bool move = false, bool gotoEat = false, bool gotoFight = false, bool escape = false, bool eat = false, bool fight = false)
        {
            CurrentState.moving = move;
            CurrentState.goToEat = gotoEat;
            CurrentState.goToFight = gotoFight;
            CurrentState.escaping = escape;
            CurrentState.eating = eat;
            CurrentState.fighting = fight;

            Actor.CurrentState = CurrentState;
        }


        // Condiciones de salida
        private IEnumerator CheckIfActorFinishWithConsumable(IConsumable consumable, StatusTypes statusType)
        {
            bool done = false;
            while (!done)
            {
                // If consumable is depleted
                if (consumable.GetSacietyPoints() <= 0)
                    done = true;

                // If actor is full
                if (IfActorIsFull(statusType))
                    done = true;

                yield return null;
            }
        }

        // Condiciones
        private bool IfActorIsFull(StatusTypes statusType)
        {
            var status = Actor.StatusInstances[statusType];
            return status.Current >= status.MaxValue;
        }

        private void CheckNextQueueDetectable()
        {
            if (Actor.DetectablesQueue.Count > 0)
            {
                if (IfActorIsFull(StatusTypes.Hungry))
                {
                    //TODO: Eliminar todos los items consumables que modifiquen el hambre
                    // habrá que distinguir según status modificable
                    Actor.DetectablesQueue.Clear();
                }
                else
                {
                    var detectable = Actor.GetCurrentDetectable<IDetectable>();
                    detectable.Detect(Actor);
                }
            }
        }

        public IEnumerator IsEatingRoutine(/*float seconds*/)
        {
            var consumable = Actor.GetCurrentDetectable<IConsumable>();

            if (consumable == null || consumable.ToString() == "null")
            {
                Actor.Behaviour.Movement.Navigator.isStopped = false;
                Actor.DetectablesQueue.Dequeue();
                UpdateStates(move: true);

                //TODO
                CheckNextQueueDetectable();
            }
            else
            {
                UpdateStates(eat: true);
                Actor.Behaviour.Movement.Navigator.isStopped = true;

                consumable.OnUpdateSatiety += Actor.RestoreHungry;
                consumable.UseItem();

                yield return CheckIfActorFinishWithConsumable(consumable, StatusTypes.Hungry);

                Actor.Behaviour.Movement.Navigator.isStopped = false;
                Actor.DetectablesQueue.Dequeue();

                consumable.OnUpdateSatiety -= Actor.RestoreHungry;
                consumable.LeaveItem();
                consumable.DestroyItem();

                UpdateStates(move: true);
                
                //TODO
                CheckNextQueueDetectable();
            }
        }

        public IEnumerator IsFightingRoutine(float seconds)
        {
            UpdateStates(fight: true);
            Actor.Behaviour.Movement.Navigator.isStopped = true;

            yield return new WaitForSeconds(seconds); //<-- dependerá de la salud del rival (TODO)

            if (CurrentState.IsFighting)
            {
                Actor.Behaviour.Movement.Navigator.isStopped = false;

                //

                UpdateStates(move: true);
            }
        }


    }
}