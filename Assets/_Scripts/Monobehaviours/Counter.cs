using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Game.Interfaces;
using Game.Managers;
using Game.Tools;

[SelectionBase]
public class Counter : MonoBehaviour, IItemStacker
{
    public ItemType ItemType;
    public int Index;
    ItemType IItemStacker.ItemType { get => ItemType; set => ItemType = value; }
    int IItemStacker.Index { get => Index; set => Index = value; }

    protected StorageManager m_StorageManager => StorageManager.Instance;
    protected Stack<IItem> m_ItemStack = new Stack<IItem>();

    #region SetRefs
    private Transform m_StackPoint;
    private Image m_Outline;
    #endregion

    protected virtual void SetReferences()
    {
        m_StackPoint = transform.FindChildByName<Transform>("Stack Point");
        m_Outline = transform.FindChildByName<Image>("Outline");
    }

    public virtual void OnPlayerEnter()
    {
        m_Outline.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutCubic);
        m_Outline.DOFade(1, 0.2f).SetEase(Ease.OutCubic);
    }
    public virtual void OnPlayerExit()
    {
        m_Outline.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
        m_Outline.DOFade(0.5f, 0.2f).SetEase(Ease.OutCubic);
    }

    #region IItemStacker
    public void ItemIn(IItem item)
    {
        m_ItemStack.Push(item);
        item.OnItemIn(m_StackPoint, m_ItemStack.Count - 1);
    }
    public virtual bool ItemOut(out IItem item)
    {
        item = null;
        if (m_ItemStack.TryPop(out IItem result))
        {
            if (result.Moving)
            {
                m_ItemStack.Push(result);
                return false;
            }
            item = result;
            return true;
        }
        else
        {
            ProductionCounter.Produce(ItemType, Index);
            return false;
        }
    }
    #endregion
}