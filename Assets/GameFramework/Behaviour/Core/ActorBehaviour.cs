using System;
using System.Linq;
using UnityEngine;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Interfaces;
using System.Collections;

namespace Assets.GameFramework.Behaviour.Core
{
    [Serializable]
    public class ActorBehaviour : MonoBehaviour, IActorBehaviour
    {
        public ActorBase Actor { get; set; }
        public IMovable Movement { get; set; }

        public States CurrentState;


        public event Func<IEnumerator> OnNextAction;

        public void CheckPosition(Vector3 position, float stoppingDistance)
        {
            //if (Vector3.Distance(transform.position, _behaviour.Movement.Target) < 3)
            if ((position - Movement.Target).sqrMagnitude < stoppingDistance * stoppingDistance)
            {
                if (CurrentState.searching)
                {
                    Actor.StartCoroutine(OnNextAction());
                }
                else
                {
                    MoveToPosition();
                }
            }
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

    //TO DO: CAMBIAR... 
    public class States
    {
        public bool searching = false;
        public bool eating = false;
        public bool moving = false;

        public void UpdateStates(bool search, bool eat, bool move)
        {
            searching = search;
            eating = eat;
            moving = move;
        }
    }

}