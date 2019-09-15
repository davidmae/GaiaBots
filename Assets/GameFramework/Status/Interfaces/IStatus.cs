using Assets.GameFramework.Actor.Core;


namespace Assets.GameFramework.Status.Interfaces
{
    public interface IStatus
    {
        void EvaluateStatus(ActorBase actor);
    }
}