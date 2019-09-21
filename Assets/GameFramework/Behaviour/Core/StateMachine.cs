using Assets.GameFramework.Actor.Core;
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
        public States States { get; set; }

        public bool IsMoving => States.moving == true;
        public bool IsSearching => States.searching == true;
        public bool IsEating => States.eating == true;


        public StateMachine(ActorBase actor)
        {
            Actor = actor;
            States = new States();
        }

        public void Detect(Collider other)
        {
            var item = other.GetComponent<Consumable>();

            if (item != null)
            {
                //Si el item es consumable (expandir para otros items!!)
                item.CurrentActor = Actor;
                item.DoAction();
            }
        }

        public void UpdateStates(bool search = false, bool eat = false, bool move = false, bool escape = false)
        {
            States.searching = search;
            States.eating = eat;
            States.moving = move;
            States.escaping = escape;
        }

        public IEnumerator StayFront(float seconds)
        {
            UpdateStates(eat: true);
            Actor.Behaviour.Movement.Navigator.isStopped = true;

            yield return new WaitForSeconds(seconds); //<-- eatingTime dependerá del item (TODO)

            if (IsEating)
            {
                Actor.Behaviour.Movement.Navigator.isStopped = false;

                Actor.StatusInstances[StatusTypes.Hungry].UpdateStatus(10);
                Actor.StatusInstances[StatusTypes.Rage].UpdateStatus(0);

                UpdateStates(move: true);
            }
        }

        
    }
}