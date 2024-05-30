using UnityEngine;
using DG.Tweening;
using Game.Interfaces;
using Game.Managers;
using Game.Data;
using Game.Tools;

public class Player : Character
{
    #region Public Properties
    #endregion

    #region Private Properties
        #region References
        private InputManager m_InputManager => InputManager.Instance;
        private StorageManager m_StorageManager => StorageManager.Instance;
        private PoolManager m_PoolManager => PoolManager.Instance;
        #endregion

    private int m_Wallet;
    #endregion

    #region Init
    public override void Start()
    {
        base.Start();
        m_Wallet = m_StorageManager.GameData.PlayerWallet;
    }

    private void OnEnable()
    {
        InputManager.OnMouseDown += startMoving;
        InputManager.OnMouseUp += stopMoving;
    }
    private void OnDisable()
    {
        InputManager.OnMouseDown -= startMoving;
        InputManager.OnMouseUp -= stopMoving;
    }
    #endregion

    #region Updates
    public override void Update()
    {
        base.Update();

        if (m_CharacterState == eCharacterState.Moving)
        {
            transform.DOLookAt(transform.position + Camera.main.transform.right * m_InputManager.Drag.x + new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * m_InputManager.Drag.y, 0.1f).SetEase(Ease.Linear);
        }
        if(m_CurrentCounter != null && Time.time > StackTimer + 0.1f)
        {
            StackTimer = Time.time;

            atCounter();
        }
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector3.down * 3;
        m_Rigidbody.angularVelocity = Vector3.zero;

        if(m_CharacterState == eCharacterState.Moving)
        {
            m_Rigidbody.velocity = Vector3.down * 3 + transform.forward * GameConfig.PlayerSettings.PlayerSpeed * Mathf.Clamp(m_InputManager.Drag.sqrMagnitude, 0, 1);
        }
    }
    #endregion

    #region Specific Methods
    private void startMoving()
    {
        ChangeState(eCharacterState.Moving);
    }
    private void stopMoving()
    {
        ChangeState(eCharacterState.Idle);
    }

    private void exitPreviousCounter()
    {
        m_CurrentCounter.OnPlayerExit();
        m_CurrentCounter = null;
    }
    #endregion


    public delegate void trigger(Counter temp);
    public static event trigger onTrigger;
    #region Physics
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(eTags.Interactable)))
        {
            onTrigger?.Invoke(other.GetComponent<Counter>());
            m_CurrentCounter = other.GetComponent<Counter>();
            m_CurrentCounter.OnPlayerEnter();
            StackTimer = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(nameof(eTags.Interactable)))
        {
            if (m_CurrentCounter != null)
                exitPreviousCounter();
        }
    }
    #endregion

    #region Interaction Logic
    private void atCounter()
    {
        switch (m_CurrentCounter.CounterType)
        {
            case eCounterType.Production:
                Pickup();
                break;
            case eCounterType.Display:
                if (m_CurrentCounter.m_DisplayCounter.IsUnlocked())
                {
                    Drop();
                }
                else
                {
                    tryUnlock();
                }
                break;
            case eCounterType.Arcade:
                if (m_CurrentCounter.m_ArcadeCounter.IsUnlocked())
                {
                    CollectCash();
                }
                else
                {
                    tryUnlock();
                }
                break;
            case eCounterType.Cash:
                if (m_CurrentCounter.m_CashCounter.IsUnlocked())
                {
                    CollectCash();
                }
                else
                {
                    tryUnlock();
                }
                break;
            case eCounterType.Unlock:
                if (!m_CurrentCounter.m_UnlockCounter.IsUnlocked())
                {
                    tryUnlock();
                }
                break;
        }
    }

    private void Pickup()
    {
        if (m_ItemStack.Count < GameConfig.PlayerSettings.StackLimit[m_StorageManager.GameData.PlayerStackLevel])
        {
            if (m_CurrentCounter.ItemOut(out var item))
            {
                ItemIn(item);
            }
        }
    }
    private void Drop()
    {
        TryDropItems(m_CurrentCounter.GetComponent<IItemStackable>());
    }
    private void CollectCash()
    {
        if (m_CurrentCounter.ItemOut(out var item))
        {
            item.OnItemIn(transform, 0);
            this.DelayedAction(() =>
            {
                m_PoolManager.Enqueue(ePoolType.Cash, item.gameObject);
            }, 0.5f);
        }
        if (m_CurrentCounter.m_CashCounter.CashCollected > 0)
        {
            m_Wallet += m_CurrentCounter.m_CashCounter.CashCollected;
            m_CurrentCounter.m_CashCounter.CashCollected = 0;
            m_StorageManager.SetWallet(m_Wallet);
        }
    }

    private void tryUnlock()
    {
        if(m_Wallet > 0 && !m_InputManager.OnMouse)
        {
            int value = m_CurrentCounter.GetComponent<IUnlockable>().TryToPay();
            if (value > 0)
            {
                m_Wallet -= value;
                m_StorageManager.SetWallet(m_Wallet);

                Transform temp = m_PoolManager.Dequeue(ePoolType.Cash, transform.position + Vector3.up, Quaternion.identity, m_CurrentCounter.transform).transform;
                temp.DOLocalJump(Vector3.down, 2, 1, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => m_PoolManager.Enqueue(ePoolType.Cash, temp.gameObject));
            }
        }
    }
    #endregion
}