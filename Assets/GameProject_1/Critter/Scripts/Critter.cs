using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
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
    [RequireComponent(typeof(Rigidbody))]
    public class Critter : ActorBase
    {
        [SerializeField] protected CritterData critterData;
        [SerializeField] protected List<StatusData> statusData;

        private IConsumable currentItem;


        private void Awake()
        {
            var navigator = GetComponent<NavMeshAgent>();

            base.Behaviour = new ActorBehaviour(this, new MovableAI(navigator), new StateMachine());
            base.StatusInstances = statusData.InitializeStatusInstancesFromStatusData();

            var sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = 2;
            sphereCollider.isTrigger = true;
             
            Behaviour.Movement.Navigator.speed = critterData.Speed;
            Behaviour.Movement.Navigator.acceleration = critterData.Acceleration;
        }

        private void Start()
        {
            Behaviour.MoveToPosition();
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

            if (hitInfo.collider != null)
            {
                //TODO: ajustes de IA 
                //Debug.Log($"Raycasthit :: {obstacle}");
            }
            #endregion

            if (Behaviour.ArrivedToPosition(transform.position, critterData.StopingDistance))
            {
                if (currentItem != null)
                    StartCoroutine(Behaviour.StateMachine.StayFront(critterData.EatingTime));
            }


            #region Debugging
            //time += Time.deltaTime;
            //if (time >= timeLimit)
            //{
            //    Debug.Log("Distance to target: " + Vector3.Distance(transform.position, _behaviour.Movement.Target));
            //    time = 0;
            //}
            #endregion
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
                Behaviour.StateMachine.Detect(other);

        }

        //public IEnumerator StayFrontItem()
        //{
        //    behaviour.CurrentState.UpdateStates(false, true, false);

        //    behaviour.Movement.Navigator.isStopped = true;
        //    yield return new WaitForSeconds(critterData.EatingTime); //<-- eatingTime dependerá del item (TODO)
        //    behaviour.Movement.Navigator.isStopped = false;

        //    if (currentItem != null)
        //    {
        //        statusInstances[StatusTypes.Hungry].UpdateStatus(10);
        //        currentItem = null;
        //    }

        //    behaviour.CurrentState.UpdateStates(false, false, true);
        //}
    }
}
