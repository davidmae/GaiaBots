using Assets.GameFramework.Behaviour.Interfaces;
using Assets.GameFramework.Movement.Core;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameFramework.Movement.Core
{
    public class MovableAI : Movable
    {
        public MovableAI(MyNavigator navigator)
        {
            Navigation = navigator;
        }

        public override void MoveToPosition(Vector3 position = new Vector3())
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

            base.MoveToPosition(position);
        }
    }
}
