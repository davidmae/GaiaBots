using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;
using Assets.GameProject_1.Status;
using Assets.GameProject_1.Status.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameProject_1.Critter.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
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
            NavMeshAgent navigator = GetComponent<NavMeshAgent>();

            Behaviour = new ActorBehaviour(this, new MovableAI(navigator), new StateMachine(this));
            StatusInstances = statusData.InitializeStatusInstancesFromStatusData();

            ListStatus = StatusInstances.Select(s => s.Value).ToList();

            Behaviour.Movement.Navigator.speed = critterData.Speed;
            Behaviour.Movement.Navigator.acceleration = critterData.Acceleration;
        }

        private void Start()
        {
            Behaviour.Movement.MoveToPosition();
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


            if (Behaviour.Movement.ArrivedToPosition(transform.position, critterData.StopingDistance))
            {
                if (Behaviour.StateMachine.CurrentState.IsGoingToEat)
                {
                    Behaviour.StateMachine.ExecuteAction(Behaviour.StateMachine.IsEatingRoutine, critterData.EatingTime);
                }
                else if (Behaviour.StateMachine.CurrentState.IsGoingToFight)
                {
                    Behaviour.StateMachine.ExecuteAction(Behaviour.StateMachine.IsFightingRoutine, 5f); //<-- TODO
                }
                else
                {
                    Behaviour.Movement.MoveToPosition();
                }
            }


            if (time >= (second * 5f))
            {
                StatusInstances[StatusTypes.Defecate].UpdateStatus(0);
                time = 0;
            }

            time += Time.deltaTime;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                var detectable = other.GetComponent<IDetectable>();
                Behaviour.StateMachine.Detect(detectable);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Behaviour.StateMachine.CurrentState.IsMoving)
                return;

            if (other != null)
            {
                var detectable = other.GetComponent<IDetectableDynamic>();
                Behaviour.StateMachine.Detect(detectable);
            }
        }
    }
}
