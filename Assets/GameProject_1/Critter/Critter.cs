using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Movement.Core;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Senses.Interfaces;
using Assets.GameFramework.Status.Core;
using Assets.GameProject_1.Senses;
using Assets.GameProject_1.States;
using Assets.GameProject_1.Status;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.GameProject_1.Critter
{
    [RequireComponent(typeof(MyNavigator))]
    [RequireComponent(typeof(Rigidbody))]
    public class Critter : ActorBase
    {
        [Header("Data initialization")]
        [SerializeField] protected CritterData critterData;
        [SerializeField] protected List<StatusData> statusData;

        float second = 1f;
        float time = 0f;


        //TODO
        public Canvas canvas;
        public GameObject containerHealth;
        public GameObject containerHungry;
        public GameObject containerTarget;
        public GameObject containerEvento;

        TextMeshProUGUI health, hungry, target, evento;
        Animator animator;
        Camera camera;

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

            ListStatus = StatusInstances.Select(s => s.Value).ToList();

            Senses = GetComponentsInChildren<SenseBase>().ToList();

            health = containerHealth.GetComponent<TextMeshProUGUI>();
            hungry = containerHungry.GetComponent<TextMeshProUGUI>();
            target = containerTarget.GetComponent<TextMeshProUGUI>();
            evento = containerEvento.GetComponent<TextMeshProUGUI>();


            animator = GetComponent<Animator>();
            camera = FindObjectOfType<Camera>();
        }

        private void Start()
        {
            Behaviour.StateMachine.Start();
        }

        private void Update()
        {
            if (animator != null)
                animator.SetInteger("Walk", Behaviour.StateMachine.CurrentState.IsMoving ? 1 : 0);

            if (canvas != null)
            {
                health.text = StatusInstances[StatusTypes.Health].Current.ToString();
                hungry.text = StatusInstances[StatusTypes.Hungry].Current.ToString();

                if (this.DetectableQueue.Count > 0)
                    target.text = this.DetectableQueue.Last().GetGameObject().name;
                else target.text = "";

                if (this.CurrentTarget != null)
                    target.text = this.CurrentTarget.GetGameObject().name;

                //evento.text = "From " + CurrentTarget.GetGameObject().name + " :: " + this.GetOnUpdateVal().Method;

                //GetInvocationList()?.GetLength(0)

                canvas.transform.LookAt(camera.transform.position);
            }
            

            //#region Debugging

            //Ray ray = new Ray(transform.position, transform.forward);
            //RaycastHit hitInfo;

            //if (Physics.Raycast(ray, out hitInfo, 100))
            //{
            //    Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            //}
            //else
            //{
            //    Debug.DrawLine(transform.position, ray.origin + ray.direction * 100, Color.green);
            //}

            //Debug.DrawLine(transform.position, ray.origin + ray.direction * attackDistance, Color.blue);

            //#endregion
        }
        
        //protected virtual void OnTriggerEnter(Collider other)
        //{
        //    if (other != null)
        //    {
        //        var detectable = other.GetComponent<IDetectable>();
        //        Senses.Detect(this, detectable);
        //    }
        //}

        //protected virtual void OnTriggerStay(Collider other)
        //{
        //    if (other != null)
        //    {
        //        if (Behaviour.StateMachine.CurrentState.IsGoingToFight)
        //        {
        //            var detectable = other.GetComponent<IDetectableDynamic>();
        //            Senses.Detect(this, detectable);
        //        }
        //    }
        //}
    }
}
