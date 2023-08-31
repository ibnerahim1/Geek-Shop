using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Game.Interfaces;
using Game.Managers;
using Game.Tools;

public class ArcadeCounter : Counter, IUnlockable
{
    #region Public Properties
    public bool IsUnlocked() { return !(UnlockCost > PaidAmount); }
    public int PaidAmount { get; internal set; }
    #endregion


    #region Private Properties
        #region References
        private GameObject m_Body;
        private GameObject m_Canvas;
        private TextMeshProUGUI m_CashText;
        private Image m_Fill;
        private BoxCollider m_BoxCollider;
        private Vector3 m_Extent;
        #endregion

    [Min(1), SerializeField] private int UnlockCost;
    private int m_UnlockIndex;
    #endregion


    #region Init
    private void OnValidate()
    {
        SetReferences();
    }

    protected override void SetReferences()
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
    }
    public override void OnPlayerExit()
    {
        base.OnPlayerExit();
        m_Canvas.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
    }
    #endregion

    #region Interfaces Methods
    public void Initialize(int i_Index)
    {
        m_UnlockIndex = i_Index;
        PaidAmount = m_StorageManager.GameData.PaidUnlockables[m_UnlockIndex];

        if (IsUnlocked())
            Invoke("Unlock", 0.1f);
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
}
