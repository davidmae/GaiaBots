using Assets.GameProject_1.Critter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameProject_1.Map
{
    public class ChunkDetector : MonoBehaviour
    {
        Collider chunkCollider;

        private void Awake()
        {
            chunkCollider = GetComponentInParent<Collider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Critter")
            {
                other.GetComponent<CritterBase>().SetCollider(chunkCollider);
            }
        }
    }
}
