using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Movement.Core;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Senses.Interfaces;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.UI;
using Assets.GameProject_1.Senses;
using Assets.GameProject_1.States;
using Assets.GameProject_1.Status;
using Pathfinding;
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
    [RequireComponent(typeof(Rigidbody))]
    public class CritterBase : ActorBase
    {
        [Header("Data initialization")]
        [SerializeField] public CritterData critterData;
        [SerializeField] public List<StatusData> statusData;

        float second = 1f;
        float time = 0f;


        //TODO
        public Canvas canvas;
        public GameObject containerHealth;
        public GameObject containerHungry;
        public GameObject containerTarget;
        public GameObject containerState;
        public GameObject containerLibido;

        TextMeshProUGUI health, hungry, target, state, libido;
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

            //var navigator = new MyNavigatorPathfind(
            //    GetComponent<AIDestinationSetter>(),
            //    GetComponent<AIPath>(),
            //    aioptions =>
            //    {
            //        aioptions.maxSpeed = critterData.Speed;
            //        aioptions.maxAcceleration = critterData.Acceleration;
            //        aioptions.endReachedDistance = critterData.StopingDistance;
            //    });


            var statesFactory = new StatesFactory(this, (factory, states) =>
            {
                states.AddState(StateMachine_BaseStates.Idle, factory.IsMovingRoutine);
                states.AddState(StateMachine_BaseStates.StayFront, factory.IsStayFrontConsumableRoutine);
                states.AddState(StateMachine_BaseStates.GoingToItem, factory.IsGoingtoDetectableItemRoutine);
                states.AddState(StateMachine_BaseStates.GoingToFight, factory.IsGoingtoFightRoutine);
                states.AddState(StateMachine_BaseStates.Attack, factory.IsFightingRoutine);
                states.AddState(StateMachine_BaseStates.StandToEntity, factory.IsStandToEntityRoutine);
                states.AddState(StateMachine_BaseStates.GoingToEntity, factory.IsGoingtoLoveRoutine);
                states.AddState(StateMachine_BaseStates.StayFrontEntity, factory.IsStayFrontEntityRoutine);
            });

            Behaviour = new ActorBehaviour()
            {
                Actor = this,
                Movement = new MovableAI(navigator) { CritterData = critterData }, //new MovableAI(navigator),
                StateMachine = new StateMachine(this, statesFactory.SelectedStates),
                HostilityBehaviour = ActorBehaviour.PrepareHostilityBehaviour(critterData.Hostility)
            };

            StatusInstances = statusData.InitializeStatusInstancesFromStatusData();
            DetectableQueue = new PriorityQueue<IDetectable>();

            ListStatus = StatusInstances.Select(s => s.Value).ToList();

            Senses = GetComponentsInChildren<SenseBase>().ToList();

            health = containerHealth.GetComponent<TextMeshProUGUI>();
            hungry = containerHungry.GetComponent<TextMeshProUGUI>();
            target = containerTarget.GetComponent<TextMeshProUGUI>();
            state = containerState.GetComponent<TextMeshProUGUI>();
            libido = containerLibido.GetComponent<TextMeshProUGUI>();

            animator = GetComponent<Animator>();
            camera = FindObjectOfType<Camera>();

            base.cursorManager = FindObjectOfType<CursorManager>();
            base.groundCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider>();

        }

        private void Start()
        {
            Behaviour.StateMachine.Start();
        }

        private void Update()
        {
            if (cursorManager?.selectedEntity != null && cursorManager.selectedEntity.name == name)
                if (selectionField != null) selectionField.enabled = true;

            if (animator != null)
                animator.SetInteger("Walk", Behaviour.StateMachine.CurrentState.IsMoving ? 1 : 0);

            if (canvas != null)
            {
                StatusBase status;
                health.text = StatusInstances.TryGetValue(StatusTypes.Health, out status) == true ? status.Current.ToString() : "";
                hungry.text = StatusInstances.TryGetValue(StatusTypes.Hungry, out status) == true ? status.Current.ToString() : "";
                libido.text = StatusInstances.TryGetValue(StatusTypes.Libido, out status) == true ? status.Current.ToString() : "";

                if (this.DetectableQueue.Count > 0)
                    target.text = this.DetectableQueue.Last().GetGameObject().name;
                else target.text = "";

                if (this.CurrentTarget != null)
                    target.text = this.CurrentTarget.GetGameObject().name;

                state.text = Behaviour.StateMachine.NextState.ToString();

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
