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

        public void Detect(IDetectable detectable)
        {
            if (detectable == null)
                return;

            if (detectable is IConsumable && IfActorIsFull(StatusTypes.Hungry))
                return;

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

        private IEnumerator CheckIfActorFinish(ActorBase detectedActor, StatusTypes statusType)
        {
            bool done = false;
            while (!done)
            {
                if (IfActorIsDeath(statusType, detectedActor))
                    done = true;

                if (IfActorIsTooFar(detectedActor))
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

        private bool IfActorIsDeath(StatusTypes statusType, ActorBase detectedActor = null)
        {
            var status = detectedActor.StatusInstances[statusType];
            return status.Current <= 0;
        }

        private bool IfActorIsTooFar(ActorBase detectedActor)
        {
            var radiusDistance = Actor.GetComponent<SphereCollider>().radius;
            var visionDistance = Actor.GetComponent<ConeCollider>().Distance;
            var detectDistance = Vector3.Distance(Actor.transform.position, detectedActor.transform.position);

            return radiusDistance < detectDistance && visionDistance < detectDistance;
        }


        private void CheckNextQueueDetectable()
        {
            if (Actor.DetectableQueue.Count > 0)
            {
                if (IfActorIsFull(StatusTypes.Hungry))
                {
                    //TODO: Eliminar todos los items consumables que modifiquen el hambre
                    // habrá que distinguir según status modificable
                    Actor.DetectableQueue.Clear();
                }
                else
                {
                    var detectable = Actor.DetectableQueue[0];
                    detectable.Detect(Actor);
                }
            }
        }

        public IEnumerator IsEatingRoutine(/*float seconds*/)
        {
            var consumable = Actor.GetCurrentDetectable<Consumable>();

            if (consumable == null || consumable.ToString() == "null")
            {
                Actor.Behaviour.Movement.Navigator.isStopped = false;
                Actor.DetectableQueue.Remove(consumable);
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
                Actor.DetectableQueue.Remove(consumable);

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
            var detectedActor = Actor.GetCurrentDetectable<ActorBase>();

            UpdateStates(fight: true);
            Actor.Behaviour.Movement.Navigator.isStopped = true;

            detectedActor.OnUpdateStatus += Actor.DamageHealth;
            detectedActor.ProcessStatus();

            yield return CheckIfActorFinish(detectedActor, StatusTypes.Health);

            Actor.Behaviour.Movement.Navigator.isStopped = false;
            Actor.DetectableQueue.Remove(detectedActor);

            detectedActor.OnUpdateStatus -= Actor.DamageHealth;
            detectedActor.EndProcessStatus();

            if (IfActorIsTooFar(detectedActor))
                Detect(detectedActor);
            else
                detectedActor.Destroy();

            UpdateStates(move: true);
        }


    }
}