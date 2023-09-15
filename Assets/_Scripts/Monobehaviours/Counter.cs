using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Game.Interfaces;
using Game.Managers;
using Game.Tools;

[SelectionBase]
public class Counter : MonoBehaviour, IItemStackable
{
    #region Public Properties
        #region Interface Properties
        eItemType IItemStackable.ItemType { get => ItemType; set => ItemType = value; }
        int IItemStackable.Index { get => Index; set => Index = value; }
        #endregion

    public eItemType ItemType;
    public int Index;
    public eCounterType CounterType => m_CounterType;

    public ProductionCounter m_ProductionCounter { get; internal set; }
    public DisplayCounter m_DisplayCounter { get; internal set; }
    public ArcadeCounter m_ArcadeCounter { get; internal set; }
    public CashCounter m_CashCounter { get; internal set; }
    public AreaUnlock m_UnlockCounter { get; internal set; }
    #endregion


    #region Private Properties
    private Transform m_StackPoint;
        #region SetRefs
        private Image m_Outline;
        [SerializeField] private eCounterType m_CounterType;
        [SerializeField] private bool m_ExtensiveStacking;
        #endregion

    protected StorageManager m_StorageManager => StorageManager.Instance;
    protected Stack<IItem> m_ItemStack = new Stack<IItem>();
    #endregion

    #region Init
    protected virtual void SetReferences()
    {
        m_StackPoint = transform.FindChildByName<Transform>("Stack Point");
        m_Outline = transform.FindChildByName<Image>("Outline");

        switch (m_CounterType)
        {
            case eCounterType.Production:
                m_ProductionCounter = GetComponent<ProductionCounter>();
                break;
            case eCounterType.Display:
                m_DisplayCounter = GetComponent<DisplayCounter>();
                break;
            case eCounterType.Arcade:
                m_ArcadeCounter = GetComponent<ArcadeCounter>();
                break;
            case eCounterType.Cash:
                m_CashCounter = GetComponent<CashCounter>();
                break;
            case eCounterType.Unlock:
                m_UnlockCounter = GetComponent<AreaUnlock>();
                break;
        }
    }
    #endregion

    #region Specific
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
    #endregion

    #region Interface Methods
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
            if (m_ItemStack.Count < 1 && CounterType == eCounterType.Display)
                ProductionCounter.Produce(ItemType, Index);
            item = result;
            return true;
        }
        else
            return false;
    }
    #endregion
}