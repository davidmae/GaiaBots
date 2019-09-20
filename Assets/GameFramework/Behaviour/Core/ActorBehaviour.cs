using System;
using System.Linq;
using UnityEngine;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Interfaces;
using System.Collections;
using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Status.Core;

namespace Assets.GameFramework.Behaviour.Core
{
    public class ActorBehaviour : MonoBehaviour, IActorBehaviour
    {
        public ActorBase Actor { get; set; }
        public IMovable Movement { get; set; }
        public StateMachine StateMachine { get; set; }

        public ActorBehaviour(ActorBase actor, IMovable movement, StateMachine stateMachine)
        {
            Actor = actor;
            Movement = movement;
            StateMachine = stateMachine;

            StateMachine.Actor = Actor;
        }

        public bool ArrivedToPosition(Vector3 position, float stoppingDistance)
        {
            if ((position - Movement.Target).sqrMagnitude < stoppingDistance * stoppingDistance)
            {
                if (StateMachine.IsSearching)
                {
                    return true;
                }
                else
                {
                    MoveToPosition();
                }
            }

            return false;
        }


        //TODO: Cambiar
        public void MoveToPosition(Vector3 position = new Vector3())
        {
            if (position == Vector3.zero)
            {
                do
                {
                    float x = UnityEngine.Random.Range(-20, 20);
                    float z = UnityEngine.Random.Range(-20, 20);
                    position = new Vector3(x, 0, z);
                }
                while (Physics.OverlapSphere(position, 4f).Where(c => c.CompareTag("Obstacle")).Count() > 0);


                // ----------------------------- For debugging ------------------------------
                //var marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                //marker.transform.localScale = new Vector3(.1f, 20f, .1f);
                //GameObject.Instantiate(marker, position, Quaternion.LookRotation(Vector3.zero));
                //GameObject.Destroy(marker, 1f);

                //Debug.Log($"Physics.CheckSphere {Actor.Name} ::: {Physics.CheckSphere(position, 4f)}");
                //Debug.Log($"Physics.OverlapSphere {Actor.Name} ::: {Physics.OverlapSphere(position, 4f)}");
            }

            Movement.SetNextTarget(position);
            Movement.MoveToTarget();
        }


        #region old_
        //[SerializeField]
        //public List<StatusBase> StatusBaseList;

        //public void SetEvaluateStatusAction<T>(Action<ActorBase> evaluate)
        //    where T : StatusBase
        //{
        //    StatusBaseList
        //        .Where(status => status is T)
        //        .FirstOrDefault()
        //        .onStatusChange += evaluate;
        //}

        //public void EvaluateStatus()
        //{
        //    foreach (var status in StatusBaseList)
        //    {
        //        status.EvaluateStatus(Actor);
        //    }
        //}
        #endregion
    }

    public class StateMachine
    {
        public ActorBase Actor { get; set; }
        public States States { get; set; }

        public StateMachine()
        {
            States = new States();
        }

        public void Detect(Collider other)
        {
            var item = other.GetComponent<Consumable>();

            if (item != null)
            {
                //Si el item es consumable (expandir para otros items!!)
                item.CurrentActor = Actor;
                item.DoAction();
            }
        }

        public void UpdateStates(bool search = false, bool eat = false, bool move = false, bool escape = false)
        {
            States.searching = search;
            States.eating = eat;
            States.moving = move;
            States.escaping = escape;
        }

        public IEnumerator StayFront(float seconds)
        {
            if (Actor != null)
            {
                Actor.Behaviour.StateMachine.UpdateStates(eat: true);

                Actor.Behaviour.Movement.Navigator.isStopped = true;
                yield return new WaitForSeconds(seconds); //<-- eatingTime dependerá del item (TODO)
                Actor.Behaviour.Movement.Navigator.isStopped = false;

                Actor.StatusInstances[StatusTypes.Hungry].UpdateStatus(10);

                UpdateStates(move: true);

                Actor = null;
            }
        }



        public bool IsMoving => States.moving == true;

        public bool IsSearching => States.searching == true;

        public bool IsEating => States.eating == true;




    }

    //TO DO: CAMBIAR... 
    public class States
    {
        public bool searching = false;
        public bool eating = false;
        public bool moving = false;
        public bool escaping = false;
    }

}