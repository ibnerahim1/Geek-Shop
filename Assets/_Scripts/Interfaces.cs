using UnityEngine;

namespace Game.Interfaces
{

    public interface IPoolable
    {
        void OnObjectSpawn();
        void OnObjectDespawn();
    }

    #region Stackables & Pickables

    public interface IItem
    {
        eItemType ItemType { get; set; }
        int Index { get; set; }
        bool Moving { get; internal set; }
        GameObject gameObject { get; }
        void OnItemIn(Transform i_StackPoint, int i_LastIndex);
        void OnItemOut();
    }
    public interface IItemStackable
    {
        eItemType ItemType { get; set; }
        int Index { get; set; }
        void ItemIn(IItem i_Item);
        bool ItemOut(out IItem i_Item);
    }
    #endregion

    public interface IUnlockable
    {
        void Initialize(int value);
        int TryToPay();
        void Unlock();
    }
}