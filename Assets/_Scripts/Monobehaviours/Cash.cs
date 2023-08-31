using UnityEngine;
using DG.Tweening;
using Game.Tools;
using Game.Interfaces;

public class Cash : MonoBehaviour, IItem, IPoolable
{
    public eItemType ItemType;
    public int Index;
    public bool Moving;

    eItemType IItem.ItemType { get => ItemType; set => ItemType = value; }
    int IItem.Index { get => Index; set => Index = value; }
    bool IItem.Moving { get => Moving; set => Moving = value; }

    [SerializeField] private float m_StackOffset;

    public void OnItemIn(Transform i_StackPoint, int i_LastIndex)
    {
        Moving = true;
        this.DelayedAction(() => Moving = false, 0.5f);
        transform.parent = i_StackPoint.GetChild(i_LastIndex % i_StackPoint.childCount);

        transform.DOLocalJump(Vector3.up * (i_LastIndex / i_StackPoint.childCount) * m_StackOffset, 2, 1, 0.5f).SetEase(Ease.InOutCubic);
        transform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic);
    }

    public void OnItemOut()
    {
        
    }

    public void OnObjectSpawn()
    {

    }

    public void OnObjectDespawn()
    {

    }
}
