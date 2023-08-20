using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Game.Interfaces;
using Game.Tools;

public class DisplayProp : Counter, IUnlockable
{
    [Min(1), SerializeField] private int UnlockCost;
    public int PaidAmount { get; internal set; }
    public int Capacity;

    public delegate void CounterUnlocked(DisplayProp displayProps);
    public static CounterUnlocked OnDisplayPropUnlocked;

    private Vector3 m_Extent;
    private int m_UnlockIndex;

    #region SetRefs
    private GameObject m_Body;
    private GameObject m_Canvas;
    private TextMeshProUGUI m_CashText;
    private Image m_Fill;
    private BoxCollider m_BoxCollider;
    #endregion

    public bool IsUnlocked() { return !(UnlockCost > PaidAmount); }

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

    private void OnValidate()
    {
        SetReferences();
    }

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
        Debug.Log($"{name} has been unlocked!");
        OnDisplayPropUnlocked?.Invoke(this);

        m_BoxCollider.size = m_Extent;
        m_Canvas.SetActive(false);
        m_Body.SetActive(true);
        m_Body.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutCubic).From(Vector3.zero);
    }

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
}
