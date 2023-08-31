using UnityEngine;
using DG.Tweening;
using Game.Tools;
using Game.Interfaces;

public class Item : MonoBehaviour, IItem, IPoolable
{
    public eItemType ItemType;
    public int Index;
    public bool Moving;

    eItemType IItem.ItemType { get => ItemType; set => ItemType = value; }
    int IItem.Index { get => Index; set => Index = value; }
    bool IItem.Moving { get => Moving; set => Moving = value; }

    public float[,] ItemOffset;
    public int ItemCost;

    [SerializeField] private float m_StackOffset;

    public void OnItemIn(Transform i_StackPoint, int i_LastIndex)
    {
        Moving = true;
        this.DelayedAction(() => Moving = false, 0.2f);
        transform.parent = i_StackPoint.GetChild(i_LastIndex % i_StackPoint.childCount);

        transform.DOLocalJump(Vector3.up * (i_LastIndex / i_StackPoint.childCount) * m_StackOffset, 2, 1, 0.2f).SetEase(Ease.InOutCubic);
        transform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.InOutCubic);
    }

    public void OnItemOut()
    {

    }

    public void OnObjectSpawn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetChildActive(); // By default disables all child objects
        }
        transform.SetChildActive(); // By default disables all child objects

        switch (ItemType)
        {
            case eItemType.Comics:
                transform.SetChildActive(0, true);
                transform.GetChild(0).SetChildActive(Index, true);
                break;
            case eItemType.Merchs:
                transform.SetChildActive(1, true);
                transform.GetChild(1).SetChildActive(Index, true);
                break;
            case eItemType.Clothes:
                transform.SetChildActive(2, true);
                transform.GetChild(2).SetChildActive(Index, true);
                break;
        }
    }

    public void OnObjectDespawn()
    {
        
    }
}
