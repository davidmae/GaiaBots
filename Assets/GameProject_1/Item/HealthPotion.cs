using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameFramework.UI;
using Assets.GameProject_1.Status;
using UnityEngine;

namespace Assets.GameProject_1.Item
{
    public class HealthPotion : Consumable<HealthStatus>
    {
        private void Awake()
        {
            base.OnGetStatusModified += () => new HealthStatus(StatusTypes.Health);
            base.cursorManager = FindObjectOfType<CursorManager>();
            base.groundCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider>();
        }

    }
}
