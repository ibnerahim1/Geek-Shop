using UnityEngine;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Tools;

[SelectionBase]
public class Character : MonoBehaviour, IItemStackable
{
    #region Public Properties
        #region Interface Properties
        eItemType IItemStackable.ItemType { get => ItemType; set => ItemType = value; }
        int IItemStackable.Index { get => Index; set => Index = value; }
        #endregion

    public eItemType ItemType;
    public int Index;
    #endregion

    #region Private Properties
        #region References
        protected Rigidbody m_Rigidbody;
        protected Animator m_Animator;
        protected Transform m_StackPoint;
        #endregion

    [SerializeField] protected float StackTimer;
    protected eCharacterState m_CharacterState;
    protected Counter m_CurrentCounter;
    protected Stack<IItem> m_ItemStack = new Stack<IItem>();
    protected static int m_MoveID = Animator.StringToHash("Move");
    #endregion

    #region Init
    protected virtual void OnValidate()
    {
        SetReferences();
    }
    protected virtual void SetReferences()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = transform.FindChildByName<Animator>("Body");
        m_StackPoint = transform.FindChildByName<Transform>("Stack Point");
    }

    protected virtual void Start()
    {
        m_CharacterState = eCharacterState.Idle;
    }
    #endregion

    #region Update
    protected virtual void Update()
    {
        m_Animator.SetFloat(m_MoveID, m_Rigidbody.velocity.magnitude);
    }
    #endregion

    #region Specific Methods
    protected virtual void ChangeState(eCharacterState i_NewState)
    {
        m_CharacterState = i_NewState;
    }

    protected virtual void IsCarrying()
    {
        m_Animator.SetLayerWeight(1, m_ItemStack.Count > 0 ? 1 : 0);
    }
    public void TryDropItems(IItemStackable i_ItemStacker)
    {
        if (m_ItemStack.TryPeek(out IItem result))
        {
            if (Helpers.DoesCounterMatchItem(i_ItemStacker, result))
            {
                i_ItemStacker.ItemIn(m_ItemStack.Pop());
                IsCarrying();
            }
        }
    }
    #endregion

    #region Interface Methods
    public void ItemIn(IItem item)
    {
        m_ItemStack.Push(item);
        item.OnItemIn(m_StackPoint, m_ItemStack.Count - 1);
        IsCarrying();
    }

    public bool ItemOut(out IItem i_Item)
    {
        i_Item = null;
        return false;
    }
    #endregion
}