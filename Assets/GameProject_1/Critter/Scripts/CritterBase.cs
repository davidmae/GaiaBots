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
    public class CritterBase : ActorBase
    {
        [SerializeField] protected CritterData  critterData;
        [SerializeField] protected List<StatusData> statusData;


        protected ActorBehaviour _behaviour;
        protected IDictionary<StatusTypes, StatusBase> statusInstances;


        private IItem currentItem;


        private void Awake()
        {
            var navigator = GetComponent<NavMeshAgent>();

            _behaviour = new ActorBehaviour()
            {
                Actor = this,
                Movement = new MovableAI() { Navigator = navigator },
                CurrentState = new States()
            };

            statusInstances = statusData.InitializeStatusInstancesFromStatusData();

            //TODO: Parametros ¿behaviour?
            var sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = 2;
            sphereCollider.isTrigger = true;


            _behaviour.Movement.Navigator.speed = critterData.Speed;
            _behaviour.Movement.Navigator.acceleration = critterData.Acceleration;
        }

        private void Start()
        {
            _behaviour.MoveToPosition();
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

            _behaviour.CheckPosition(transform.position, critterData.StopingDistance);

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
            {
                var item = other.GetComponent<Consumable>();

                if (item != null)
                {
                    //Si el item es consumable (expandir para otros items!!)

                    currentItem = item;

                    _behaviour.OnNextAction += StayFrontItem;
                    _behaviour.Movement.SetNextTarget(item.transform.position);
                    _behaviour.MoveToPosition(item.transform.position);

                    _behaviour.CurrentState.UpdateStates(true, false, false);

                }
            }
        }

        public IEnumerator StayFrontItem()
        {
            _behaviour.CurrentState.UpdateStates(false, true, false);

            _behaviour.Movement.Navigator.isStopped = true;
            yield return new WaitForSeconds(critterData.EatingTime); //<-- eatingTime dependerá del item (TODO)
            _behaviour.Movement.Navigator.isStopped = false;

            if (currentItem != null)
            {
                statusInstances[StatusTypes.Hungry].UpdateStatus(10);
                currentItem = null;
            }

            _behaviour.CurrentState.UpdateStates(false, false, true);
        }
    }
}
