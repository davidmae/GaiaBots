using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Behaviour.Core;
using Assets.GameFramework.Common;
using Assets.GameFramework.Item.Interfaces;
using Assets.GameFramework.Movement.Core;
using Assets.GameFramework.Senses.Core;
using Assets.GameFramework.Senses.Interfaces;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.UI;
using Assets.GameProject_1.Senses;
using Assets.GameProject_1.States;
using Assets.GameProject_1.Status;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.GameProject_1.Critter
{
    public class CritterPointClickMove : MonoBehaviour
    {
        public CritterBase critter;
        public bool positionReached;
        public Vector3 targetPosition;

        private void Awake()
        {
            critter = GetComponent<CritterBase>();
        }

        void Start()
        {
            positionReached = false;
        }

        void Update()
        {
            if (critter.cursorManager.selectedEntity == null)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    targetPosition = hit.point;
                    positionReached = false;
                    critter.Behaviour.Movement.MoveToPosition(targetPosition);
                }
            }
            
            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                positionReached = true;
            }
        }

    }
}
