using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.UI;
using Assets.GameProject_1.Status;
using UnityEngine;

namespace Assets.GameProject_1.Item.Scripts
{
    public class Apple : Consumable<HungryStatus>
    {
        private void Awake()
        {
            base.OnGetStatusModified += () => new HungryStatus(StatusTypes.Hungry);
            base.cursorManager = FindObjectOfType<CursorManager>();
            base.groundCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider>();
        }

    }
}

