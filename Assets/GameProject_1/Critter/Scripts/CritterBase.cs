using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Item.Core;
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
    public class CritterBase : MonoBehaviour
    {
        protected ActorBehaviour _behaviour;

        [SerializeField]
        protected CritterData CritterData;


        public float time = 0;
        public float timeLimit = 0.5f;
        
        //TODO: Behaviour AI for State management
        public bool searching = false;
        public bool eating = false;

        bool inCoroutine = false;

        private void Awake()
        {
            var hungryStatus = new HungryStatus(75, 100);
            var rageStatus = new RageStatus(75, 100);
            var navigator = GetComponent<NavMeshAgent>();

            CritterData.Name = "CritterBase";

            _behaviour = new ActorBehaviour()
            {
                Actor = new ActorBase() { Name = "CritterBase" },
                Movement = new MovableAI() { Navigator = navigator },
                StatusBaseList = new List<StatusBase>() { hungryStatus, rageStatus }
            };

            searching = true;
            eating = false;


            //TODO: Parametros ¿behaviour?
            var sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = 2;
            sphereCollider.isTrigger = true;

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
                //Debug.Log($"Raycasthit :: {obstacle}");
            }


            //TODO: Cambiar parametro
            if (Vector3.Distance(transform.position, _behaviour.Movement.Target) < 3)
            {
                Debug.Log($"{gameObject.name} :: Arrived to position! --- Going to new random position ---");

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
            yield return new WaitForSeconds(CritterData.EatingTime);
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
