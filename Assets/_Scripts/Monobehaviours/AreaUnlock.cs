using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Game.Interfaces;
using Game.Managers;
using Game.Tools;

public class AreaUnlock : Counter, IUnlockable
{
    public GameObject[] DisableObjects, EnableObjects;

    [Min(1), SerializeField] private int UnlockCost;
    public int PaidAmount { get; internal set; }
    private int m_UnlockIndex;

    #region SetRefs
    private GameObject m_Canvas;
    private TextMeshProUGUI m_CashText;
    private Image m_Fill;
    #endregion

    public bool IsUnlocked() { return !(UnlockCost > PaidAmount); }

    protected override void SetReferences()
    {
        m_Canvas = transform.FindChildByName<Transform>("Canvas").gameObject;
        m_CashText = transform.FindChildByName<TextMeshProUGUI>("Cash Text");
        m_Fill = transform.FindChildByName<Image>("Fill");
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
            Invoke("Unlock", 0.1f);
        else
        {
            m_CashText.text = (UnlockCost - PaidAmount).ToString();
            m_Fill.fillAmount = PaidAmount / (float)UnlockCost;
            m_Canvas.SetActive(true);

            for (int i = 0; i < EnableObjects.Length; i++)
            {
                EnableObjects[i].SetActive(false);
            }
            for (int i = 0; i < DisableObjects.Length; i++)
            {
                DisableObjects[i].SetActive(true);
            }
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
        this.DelayedAction(() => LevelManager.BuildNavMesh(), 0.2f);
        m_Canvas.SetActive(false);

        for (int i = 0; i < EnableObjects.Length; i++)
        {
            EnableObjects[i].SetActive(true);
        }
        for (int i = 0; i < DisableObjects.Length; i++)
        {
            DisableObjects[i].SetActive(false);
        }
        Destroy(gameObject);
    }

    public override void OnPlayerEnter()
    {
        m_Canvas.transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.OutCubic);
    }
    public override void OnPlayerExit()
    {
        m_Canvas.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
    }
}
