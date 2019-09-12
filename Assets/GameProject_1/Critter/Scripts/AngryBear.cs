using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Status.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameProject_1.Status.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(SphereCollider))] 
    public class AngryBear : MonoBehaviour
    {
        protected ActorBehaviour _behaviour;

        public float time = 0;
        public float timeLimit = 0.5f;
        
        //TODO: En función de más cosas...
        public int eatingTime = 3;
        
        //TODO: Behaviour AI for State management
        public bool searching = false;
        public bool eating = false;

        bool inCoroutine = false;

        private void Awake()
        {
            var hungryStatus = new HungryStatus(75, 100);
            var rageStatus = new RageStatus(75, 100);
            var navigator = GetComponent<NavMeshAgent>();

            _behaviour = new ActorBehaviour()
            {
                Actor = new BaseActor() { Name = "AngryBear" },
                Movement = new MovableAI() { Navigator = navigator },
                StatusBaseList = new List<StatusBase>() { hungryStatus, rageStatus }
            };

            searching = true;
            eating = false;


            //_behaviour.SetEvaluateStatusAction<HungryStatus>(hungryStatus.IsHungry);
            //_behaviour.SetEvaluateStatusAction<RageStatus>(rageStatus.IsRage);

            ////...
            //_behaviour.EvaluateStatus();
        }

        private void Start()
        {
            _behaviour.DoNextMovement();
        }

        private void Update()
        {
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
            }


            if (Vector3.Distance(transform.position, _behaviour.Movement.Target) < 4)
            {
                Debug.Log("Arrived to position! --- Going to new random position ---");

                if (eating)
                {
                    StartCoroutine(IsEating());
                }
                else
                {
                    _behaviour.DoNextMovement();
                }
            }

            time += Time.deltaTime;

            if (time >= timeLimit)
            {
                Debug.Log(Vector3.Distance(transform.position, _behaviour.Movement.Target));
                time = 0;
            }
        }

        private IEnumerator IsEating()
        {
            _behaviour.Movement.Navigator.isStopped = true;
            yield return new WaitForSeconds(eatingTime);
            _behaviour.Movement.Navigator.isStopped = false;
            eating = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Colission with: " + other);

            if (other != null)
            {
                var item = other.GetComponent<Consumable>();

                if (item != null)
                {
                    //transform.LookAt(item.transform);
                    _behaviour.Movement.SetNextTarget(item.transform.position);
                    _behaviour.DoNextMovement(item.transform.position);

                    searching = false;
                    eating = true;

                }
            }

        }

    }
}