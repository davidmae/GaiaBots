using Assets.GameFramework;
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
    public class TrapController : GFrameworkEntityBase
    {
        private void Start()
        {
            base.cursorManager = FindObjectOfType<CursorManager>();
            base.groundCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider>();
        }

        private void OnMouseDown()
        {
            var door = GetComponentsInChildren<Rigidbody>().FirstOrDefault(t => t.name == "Door");
            door.useGravity = true;
        }

    }
}
