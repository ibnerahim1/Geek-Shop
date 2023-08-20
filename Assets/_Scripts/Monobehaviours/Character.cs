using UnityEngine;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Tools;

[SelectionBase]
public class Character : MonoBehaviour, IItemStacker
{
    protected static int m_MoveID = Animator.StringToHash("Move");

    public ItemType ItemType;
    public int Index;
    ItemType IItemStacker.ItemType { get => ItemType; set => ItemType = value; }
    int IItemStacker.Index { get => Index; set => Index = value; }

    [SerializeField] protected float StackTimer;
    protected CharacterState m_CharacterState;

    protected Rigidbody m_Rigidbody;
    protected Animator m_Animator;


    protected Transform m_StackPoint;
    protected Stack<IItem> m_ItemStack = new Stack<IItem>();
    protected Counter m_CurrentCounter;

    protected virtual void SetReferences()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = transform.FindChildByName<Animator>("Body");
        m_StackPoint = transform.FindChildByName<Transform>("Stack Point");
    }

    protected virtual void OnValidate()
    {
        SetReferences();
    }

    protected virtual void Start()
    {
        m_CharacterState = CharacterState.Idle;
    }

    protected virtual void Update()
    {
        m_Animator.SetFloat(m_MoveID, m_Rigidbody.velocity.magnitude);
    }

    protected virtual void ChangeState(CharacterState i_NewState)
    {
        m_CharacterState = i_NewState;
    }

    protected virtual void IsCarrying()
    {
        m_Animator.SetLayerWeight(1, m_ItemStack.Count > 0 ? 1 : 0);
    }


    public void TryDropItems(IItemStacker i_ItemStacker)
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

    #region IItemStacker
    public void ItemIn(IItem item)
    {
        IsCarrying();
        m_ItemStack.Push(item);
        item.OnItemIn(m_StackPoint, m_ItemStack.Count - 1);
    }

    public bool ItemOut(out IItem i_Item)
    {
        i_Item = null;
        return false;
    }
    #endregion
}
