using Assets.GameFramework.Item.Core;
using Assets.GameFramework.Status.Core;
using Assets.GameProject_1.Status;

namespace Assets.GameProject_1.Item.Scripts
{
    public class Apple : Consumable<HungryStatus>
    {
        private void Awake()
        {
            base.OnGetStatusModified += () => new HungryStatus(StatusTypes.Hungry);
        }
    }

}
