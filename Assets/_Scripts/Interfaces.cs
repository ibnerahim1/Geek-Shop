using UnityEngine;

namespace Game.Interfaces
{

    public interface IPoolable
    {
        // Method to activate the object when retrieved from the pool
        void OnObjectSpawn();

        // Method to deactivate the object when returned to the pool
        void OnObjectDespawn();
    }


    #region Stackables & Pickables

    public interface IItem
    {
        ItemType ItemType { get; set; }
        int Index { get; set; }
        bool Moving { get; internal set; }
        GameObject gameObject { get; }
        void OnItemIn(Transform i_StackPoint, int i_LastIndex);
        void OnItemOut();
    }
    public interface IItemStacker
    {
        ItemType ItemType { get; set; }
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