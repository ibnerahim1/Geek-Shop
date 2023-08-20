using UnityEngine;
using DG.Tweening;
using Game.Interfaces;
using Game.Managers;
using Game.Data;

public class Player : Character
{
    private InputManager m_InputManager => InputManager.Instance;
    private StorageManager m_StorageManager => StorageManager.Instance;
    private PoolManager m_PoolManager => PoolManager.Instance;

    private int m_Wallet;

    protected override void Start()
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

    protected override void Update()
    {
        base.Update();

        if (m_CharacterState == CharacterState.Moving && m_InputManager.Drag.sqrMagnitude > 0.01f)
        {
            transform.DOLookAt(transform.position + Helpers.MainCamera.transform.right * m_InputManager.Drag.x + new Vector3(Helpers.MainCamera.transform.forward.x, 0, Helpers.MainCamera.transform.forward.z) * m_InputManager.Drag.y, 0.1f).SetEase(Ease.Linear);
        }

        if(m_CurrentCounter != null && Time.time > StackTimer + 0.1f)
        {
            StackTimer = Time.time;

            atCounter();
        }
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;


        if(m_CharacterState == CharacterState.Moving)
        {
            m_Rigidbody.velocity = transform.forward * GameConfig.PlayerSettings.PlayerSpeed * Mathf.Clamp(m_InputManager.Drag.sqrMagnitude, 0, 1);
        }
    }

    private void startMoving()
    {
        ChangeState(CharacterState.Moving);


    }
    private void stopMoving()
    {
        ChangeState(CharacterState.Idle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(eTags.Counter)))
        {
            if (m_CurrentCounter != null)
                exitPreviousCounter();
            m_CurrentCounter = other.GetComponent<Counter>();
            m_CurrentCounter.OnPlayerEnter();
            StackTimer = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(nameof(eTags.Counter)))
        {
            if (m_CurrentCounter != null)
                exitPreviousCounter();
        }
    }

    private void exitPreviousCounter()
    {
        m_CurrentCounter.OnPlayerExit();
        m_CurrentCounter = null;
    }

    private void atCounter()
    {
        if (m_CurrentCounter.GetComponent<CashCounter>())
        {
            if (m_CurrentCounter.GetComponent<CashCounter>().IsUnlocked())
            {
                Pickup();
            }
            else
            {
                tryUnlock();
            }

        }
        if (m_CurrentCounter.GetComponent<DisplayCounter>())
        {
            if (m_CurrentCounter.GetComponent<DisplayCounter>().IsUnlocked())
            {
                Drop();
            }
            else
            {
                tryUnlock();
            }
        }
        if (m_CurrentCounter.GetComponent<ProductionCounter>())
        {
            Pickup();
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
        TryDropItems(m_CurrentCounter.GetComponent<IItemStacker>());
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

                Transform temp = m_PoolManager.Dequeue(PoolType.Cash, transform.position + Vector3.up, Quaternion.identity, m_CurrentCounter.transform).transform;
                temp.DOLocalJump(Vector3.down, 2, 1, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => m_PoolManager.Enqueue(PoolType.Cash, temp.gameObject));
            }
        }
    }
}