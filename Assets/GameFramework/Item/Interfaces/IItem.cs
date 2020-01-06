
using System;

namespace Assets.GameFramework.Item.Interfaces
{
    public interface IItem : IGFrameworkEntityBase, IEquatable<IItem>
    {
        object GetItemFields();
    }
}
