using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameFramework.Movement.Core
{
    //[RequireComponent(typeof(NavMeshAgent))]
    public class MyNavigatorPathfind : MyNavigator
    {
        Vector3 currentTarget;

        public MyNavigatorPathfind() { }

        public MyNavigatorPathfind(NavMeshAgent navigator, Action<NavMeshAgent> navoptions) : base(navigator, navoptions)
        {
        }

        public MyNavigatorPathfind(AIDestinationSetter aiSetter, AIPath aiPath, Action<AIPath> aipathoptions)
        {
            this.Destinator = aiSetter;
            this.PathAI = aiPath;
            aipathoptions(this.PathAI);
        }

        public AIDestinationSetter Destinator { get; set; }
        public AIPath PathAI { get; set; }

        public override void Restart() => Destinator.position = currentTarget;
        public override void Stop() => Destinator.position = Destinator.position;
        public override void SetDestination(Vector3 target) { Destinator.position = target; currentTarget = target; }
        public override void SetSpeed(float speed) => PathAI.maxSpeed = speed;
        public override float GetSpeed() => PathAI.maxSpeed;

    }
}
