using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Core;
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
            if (detectable != null)
                detectable.Detect(Actor);

        }

        //public void DoNextAction()
        //{
        //    Actor.StartCoroutine(NextAction());
        //}

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

        public IEnumerator IsEatingRoutine(float seconds)
        {
            UpdateStates(eat: true);
            Actor.Behaviour.Movement.Navigator.isStopped = true;

            yield return new WaitForSeconds(seconds);

            if (CurrentState.IsEating)
            {
                Actor.Behaviour.Movement.Navigator.isStopped = false;

                Actor.StatusInstances[StatusTypes.Hungry].UpdateStatus(10);
                Actor.StatusInstances[StatusTypes.Rage].UpdateStatus(0);

                UpdateStates(move: true);
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