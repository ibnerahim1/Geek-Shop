using UnityEngine;
using DG.Tweening;
using Game.Tools;
using Game.Interfaces;

public class Item : MonoBehaviour, IItem, IPoolable
{
    public ItemType ItemType;
    public int Index;
    public bool Moving;

    ItemType IItem.ItemType { get => ItemType; set => ItemType = value; }
    int IItem.Index { get => Index; set => Index = value; }
    bool IItem.Moving { get => Moving; set => Moving = value; }

    public float[,] ItemOffset;
    public int ItemCost;

    [SerializeField] private float m_StackOffset;

    public void OnItemIn(Transform i_StackPoint, int i_LastIndex)
    {
        Moving = true;
        this.DelayedAction(() => Moving = false, 0.5f);
        transform.parent = i_StackPoint.GetChild(i_LastIndex % i_StackPoint.childCount);

        transform.DOLocalJump(Vector3.up * (i_LastIndex / i_StackPoint.childCount) * m_StackOffset, 2, 1, 0.5f).SetEase(Ease.InOutCubic);
        transform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
        //transform.DOScale(Vector3.one * 2, 0.5f).SetEase(Ease.InOutCubic);
    }

    public void OnItemOut()
    {

    }

    public void OnObjectSpawn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetChildActive();
        }
        transform.SetChildActive();

        switch (ItemType)
        {
            case ItemType.Comics:
                transform.SetChildActive(0, true);
                transform.GetChild(0).SetChildActive(Index, true);
                break;
            case ItemType.Merchs:
                transform.SetChildActive(1, true);
                transform.GetChild(1).SetChildActive(Index, true);
                break;
            case ItemType.Clothes:
                transform.SetChildActive(2, true);
                transform.GetChild(2).SetChildActive(Index, true);
                break;
            case ItemType.TCG:
                transform.SetChildActive(3, true);
                transform.GetChild(3).SetChildActive(Index, true);
                break;
        }
    }

    public void OnObjectDespawn()
    {
        
    }
}
