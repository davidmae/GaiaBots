using Assets.GameFramework.Actor.Core;
using Assets.GameFramework.Senses.Core;

namespace Assets.GameFramework.Status.Interfaces
{
    public interface IStatus
    {
        void UpdateStatus(RadiusSense senseFrom);
        void UpdateStatus(int value);
        void UpdateStatus(float value);
        bool LimitReached();
    }
}