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
        [SerializeField]
        protected ActorBehaviour _behaviour;

        [SerializeField]
        protected CritterData critterData;

        [SerializeField]
        protected List<StatusData> _critterStatus;


        public float time = 0;
        public float timeLimit = 0.5f;
        
        //TODO: Behaviour AI for State management
        public bool searching = false;
        public bool eating = false;

        bool inCoroutine = false;

        private void Awake()
        {
            var navigator = GetComponent<NavMeshAgent>();

            _behaviour = new ActorBehaviour()
            {
                Actor = new ActorBase() { Name = "CritterBase" },
                Movement = new MovableAI() { Navigator = navigator }
            };

            searching = true;
            eating = false;


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
            //if (Vector3.Distance(transform.position, _behaviour.Movement.Target) < 3)
            if ((transform.position - _behaviour.Movement.Target).sqrMagnitude < 3 * 3)
            {
                Debug.Log($"{gameObject.name} :: Arrived to position! --- Going to new random position ---");

                if (eating)
                {
                    StartCoroutine(IsEating());
                }
                else
                {
                    _behaviour.MoveToPosition();
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

            //_critterStatus[StatusData.StatusTypes.Hungry].UpdateStatus();
            _critterStatus.Where(s => s.Type == StatusData.StatusTypes.Hungry).FirstOrDefault()
                .Status.UpdateStatus(10);

            yield return new WaitForSeconds(critterData.EatingTime);
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
                    _behaviour.MoveToPosition(item.transform.position);

                    searching = false;
                    eating = true;

                }
            }

        }
    }
}
