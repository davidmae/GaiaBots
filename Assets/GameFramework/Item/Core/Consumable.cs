using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameFramework.Item.Core
{
    public class Consumable : MonoBehaviour, IConsumable
    {
        public ActorBase CurrentActor { get; set; }

        public string satiety;

        public void DoAction()
        {
            CurrentActor.Behaviour.Movement.MoveToPosition(transform.position);
            CurrentActor.Behaviour.StateMachine.UpdateStates(search: true);
        }

    }
}
