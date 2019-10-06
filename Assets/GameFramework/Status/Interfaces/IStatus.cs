using Assets.GameFramework.Actor.Core;


namespace Assets.GameFramework.Status.Interfaces
{
    public interface IStatus
    {
        void UpdateStatus(int value);
        int UpdateStatus(int value, string msg = "");
    }
}