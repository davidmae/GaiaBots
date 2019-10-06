using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Movement.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameProject_1.Senses;
using Assets.GameProject_1.States;
using Assets.GameProject_1.Status;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameProject_1.Critter
{
    [RequireComponent(typeof(MyNavigator))]
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(ConeCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Critter : ActorBase
    {
        [Header("Data initialization")]
        [SerializeField] protected CritterData critterData;
        [SerializeField] protected List<StatusData> statusData;

        float second = 1f;
        float time = 0f;

        private void Awake()
        {
            var navigator = new MyNavigator(GetComponent<NavMeshAgent>(), navoptions =>
            {
                navoptions.speed = critterData.Speed;
                navoptions.acceleration = critterData.Acceleration;
                navoptions.stoppingDistance = critterData.StopingDistance;
            });

            var statesFactory = new StatesFactory(this, (factory, states) =>
            {
                states.AddState(StateMachine_BaseStates.Idle, factory.IsMovingRoutine);
                states.AddState(StateMachine_BaseStates.StayFront, factory.IsStayfrontRoutine);
                states.AddState(StateMachine_BaseStates.GoingToItem, factory.IsGoingtoDetectableItemRoutine);
                states.AddState(StateMachine_BaseStates.GoingToFight, factory.IsGoingtoFightRoutine);
                states.AddState(StateMachine_BaseStates.Attack, factory.IsFightingRoutine);
            });


            var actor = this;
            var movement = new MovableAI(navigator);
            var stateMachine = new StateMachine(actor, statesFactory.SelectedStates);


            Behaviour = new ActorBehaviour(actor, movement, stateMachine);
            StatusInstances = statusData.InitializeStatusInstancesFromStatusData();
            DetectableQueue = new PriorityQueue<IDetectable>();
            Senses = new Sensor();

            ListStatus = StatusInstances.Select(s => s.Value).ToList();
        }

        private void Start()
        {
            Behaviour.StateMachine.Start();
        }

        private void Update()
        {
            #region Debugging

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100))
            {
                Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            }
            else
            {
                Debug.DrawLine(transform.position, ray.origin + ray.direction * 100, Color.green);
            }

            #endregion
        }


        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                var detectable = other.GetComponent<IDetectable>();
                Senses.Detect(this, detectable);
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other != null)
            {
                if (Behaviour.StateMachine.CurrentState.IsGoingToFight)
                {
                    var detectable = other.GetComponent<IDetectableDynamic>();
                    Senses.Detect(this, detectable);
                }
            }
        }
    }
}
