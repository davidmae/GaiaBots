using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameProject_1.Status;

namespace Assets.GameProject_1.Item
{
    public class HealthPotion : Consumable<HealthStatus>
    {
        private void Awake()
        {
            base.OnGetStatusModified += () => new HealthStatus(StatusTypes.Health);
        }
    }
}
