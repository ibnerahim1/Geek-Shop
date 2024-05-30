using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Game.Interfaces;
using Game.Managers;
using Game.Tools;

public class CashCounter : Counter, IUnlockable
{
    #region Public Properties

    public int PaidAmount { get; internal set; }
    public int CashCollected { get; internal set; }
    public int CashiersNearby { get; internal set; }

    public bool IsUnlocked() { return !(UnlockCost > PaidAmount); }
    public List<Customer> m_Customers { get; internal set; } = new List<Customer>();
    #endregion


    #region Private Properties
        #region References Properties
        private GameObject m_Body;
        private GameObject m_Canvas;
        private TextMeshProUGUI m_CashText;
        private Image m_Fill;
        private BoxCollider m_BoxCollider;
        private Vector3 m_Extent;
        #endregion

    [Min(0), SerializeField] private int UnlockCost;
    private int m_UnlockIndex;
    #endregion


    #region Init
    private void Awake()
    {
        SetReferences();
    }
    public override void SetReferences()
    {
        base.SetReferences();

        m_Body = transform.FindChildByName<Transform>("Body").gameObject;
        m_Canvas = transform.FindChildByName<Transform>("Canvas").gameObject;
        m_CashText = transform.FindChildByName<TextMeshProUGUI>("Cash Text");
        m_Fill = transform.FindChildByName<Image>("Fill");
        m_BoxCollider = GetComponent<BoxCollider>();
        m_Extent = m_BoxCollider.size;
    }
    #endregion

    #region Specific Calls
    public override void OnPlayerEnter()
    {
        base.OnPlayerEnter();

        m_Canvas.transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.OutCubic);
        CashiersNearby++;
    }
    public override void OnPlayerExit()
    {
        base.OnPlayerExit();

        m_Canvas.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
        CashiersNearby--;
    }
    public void OnBotEnter()
    {
        CashiersNearby++;
    }
    public void OnBotExit()
    {
        CashiersNearby--;
    }
    #endregion

    #region Interfaces Methods
    public void Initialize(int i_Index)
    {
        m_UnlockIndex = i_Index;
        PaidAmount = m_StorageManager.GameData.PaidUnlockables[m_UnlockIndex];

        if (IsUnlocked())
            Unlock();
        else
        {
            m_CashText.text = (UnlockCost - PaidAmount).ToString();
            m_Fill.fillAmount = PaidAmount / (float)UnlockCost;
            m_BoxCollider.size = Vector3.one * 2.5f;
        }
    }

    public int TryToPay()
    {
        if (IsUnlocked())
            return 0;

        int value = Mathf.Min((int)(UnlockCost / 20) + 1, UnlockCost - PaidAmount);

        PaidAmount += value;
        m_StorageManager.SetPaidUnlockables(m_UnlockIndex, PaidAmount);
        m_CashText.text = (UnlockCost - PaidAmount).ToString();
        m_Fill.fillAmount = PaidAmount / (float)UnlockCost;

        if (IsUnlocked())
        {
            Unlock();
        }
        return value;
    }

    public void Unlock()
    {
        LevelManager.Instance.AddCounters(this);

        m_BoxCollider.size = m_Extent;
        m_Canvas.SetActive(false);
        m_Body.SetActive(true);
        m_Body.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutCubic).From(Vector3.zero);
    }
    #endregion

    #region Customer Interaction Logic
    public void PayBill(int cash, IItem item)
    {
        CashCollected += cash;
        ItemIn(item);
    }
    public Vector3 GetPosition(Customer customer)
    {
        m_Customers.Add(customer);
        return transform.position + transform.forward * 1.5f * (m_Customers.Count + 1.5f);
    }
    public void RemoveCustomer(Customer customer)
    {
        this.DelayedAction(() =>
        {
            m_Customers.Remove(customer);
            for (int i = 0; i < m_Customers.Count; i++)
            {
                m_Customers[i].SetDestination(transform.position + transform.forward * 1.5f * (i + 2.5f));
            }
        }, 1);
    }
    public bool IsFirstInQueue(Customer i_Customer)
    {
        return m_Customers.Count > 0 ? m_Customers[0] == i_Customer : false;
    }
    #endregion
}