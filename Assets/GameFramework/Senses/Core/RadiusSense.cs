using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Senses.Interfaces;
using UnityEngine;

namespace Assets.GameFramework.Senses.Core
{
    public class RadiusSense : SenseBase
    {
        public float Radius;

        private void Update()
        {
            var colliders = Physics.OverlapSphere(transform.position, Radius);

            foreach (var collider in colliders)
            {
                var detectable = collider.GetComponent<IDetectable>();
                Detect(Actor, detectable);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            #region Debugging

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Radius);

            #endregion
        }
    }
}
