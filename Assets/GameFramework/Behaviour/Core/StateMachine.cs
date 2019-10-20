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
    public enum StateMachine_BaseStates
    {
        Undefined       = -1,
        Idle            =  0,
        GoingToItem     =  1,
        GoingToFight    =  2,
        StayFront       =  3,
        Attack          =  4
    }

    public class StateMachine
    {
        public ActorBase Actor { get; set; }
        public States CurrentState { get; set; }


        public StatesDictionary SelectedStates { get; private set; }
        public StateMachine_BaseStates NextState { get; set; } = StateMachine_BaseStates.Idle;


        public StateMachine(ActorBase actor, StatesDictionary states)
        {
            Actor = actor;
            SelectedStates = states;

            CurrentState = new States();
        }

        public void Start()
        {
            ExecuteAction(SelectedStates[StateMachine_BaseStates.Idle]);
        }

        public void Update()
        {
            Actor.StopAllCoroutines();
            ExecuteAction(SelectedStates[NextState]);
        }

        public void ExecuteAction(Func<IEnumerator> action) // para más tipos de datos ¿? TODO
        {
            Actor.StartCoroutine(action());
        }

        public void UpdateStates(bool move = false, bool gotoItem = false, bool gotoFight = false, bool escape = false, bool stayfront = false, bool fight = false)
        {
            CurrentState.moving = move;
            CurrentState.goToItem = gotoItem;
            CurrentState.goToFight = gotoFight;
            CurrentState.escaping = escape;
            CurrentState.stayFront = stayfront;
            CurrentState.fighting = fight;

            Actor.CurrentState = CurrentState;
        }

        
    }
}