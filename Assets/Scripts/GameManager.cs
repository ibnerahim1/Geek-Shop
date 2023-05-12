using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public bool gameStarted;
    public Transform AIObj;
    public int cash;
    public AudioSource soundfx;
    public AudioClip unlockClip, stackingClip, unstackingClip;
    public enum hapticTypes {soft, success, medium};
    public hapticTypes haptics;

    [Header("UI")]
    public GameObject menuPanel, gamePanel, settingPanel, soundImg, hapticImg;

    //[HideInInspector]
    public int sound, haptic;
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public bool UIEngaged;

    private Camera cam;
    private CoinMagnet coinMagnet;

    private void Awake()
    {
        LoadData();
    }
    private void Start()
    {
        cam = Camera.main;
        player = FindObjectOfType<Player>();
        coinMagnet = GetComponent<CoinMagnet>();
        UpdateUI();

        InitSettings();
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    void Update()
    {
        #region MyDebug
#if UNITY_EDITOR
        if (Input.GetKeyDown("b"))
            Debug.Break();
        if (Input.GetKeyDown("d"))
            PlayerPrefs.DeleteKey("level");
        if (Input.GetKeyDown("d") && Input.GetKey(KeyCode.LeftShift))
            PlayerPrefs.DeleteAll();
        if (Input.GetMouseButtonDown(1))
            SceneManager.LoadScene(0);
        //if (Input.GetKeyDown("n"))
        //{
        //    for (int i = 0; i < levels[level].areas.Length; i++)
        //    {
        //        levels[level].areas[i].unlocked = true;
        //    }
        //    for (int i = 0; i < levels[level].layers.Length; i++)
        //    {
        //        for (int j = 0; j < levels[level].layers[i].houses.Length; j++)
        //        {
        //            levels[level].layers[i].houses[j].unlocked = true;
        //        }
        //    }
        //    layer = levels[level].layers.Length - 1;
        //    CheckWin();
        //}
#endif
#endregion
        if(Input.GetMouseButtonDown(0) && !gameStarted)
        {
            gameStarted = true;
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
    }

    private void InitSettings()
    {
        sound = PlayerPrefs.HasKey("sound")? PlayerPrefs.GetInt("sound") : 1;
        haptic = PlayerPrefs.HasKey("haptic") ? PlayerPrefs.GetInt("haptic") : 1;
        soundImg.SetActive(sound == 0);
        hapticImg.SetActive(haptic == 0);
        FindObjectOfType<AudioListener>().enabled = (sound == 1);
    }



    public void UpdateUI()
    {
        //float cash = Mathf.Clamp(player.cash, 0, Mathf.Infinity);
        //if (cash < 1)
        //    cashTxt.text = Mathf.RoundToInt(cash * 1000).ToString();
        //else if (cash < 10)
        //    cashTxt.text = Mathf.RoundToInt(cash * 10) / 10 + "K";
        //else
        //    cashTxt.text = Mathf.RoundToInt(cash) + "K";
    }

    public void LoadData()
    {
        //for (int i = 0; i < levels.Length; i++)
        //{
        //    levels[i].unlocked = PlayerPrefs.HasKey("maps" + i.ToString()) ? (PlayerPrefs.GetInt("maps" + i.ToString()) == 1) : levels[i].unlocked;
        //    if (levels[i].unlocked)
        //    {
        //        mapButtons[i].GetChild(0).gameObject.SetActive(true);
        //        mapButtons[i].GetChild(1).gameObject.SetActive(false);
        //    }
        //}
        //for (int i = 0; i < layer; i++)
        //{
        //    levels[level].areas[i].unlocked = true;
        //}
    }
    public void SaveData()
    {
        //for (int i = 0; i < levels.Length; i++)
        //{
        //    PlayerPrefs.SetInt("maps" + i.ToString(), levels[i].unlocked ? 1 : 0);
        //}
        //for (int i = 0; i < houses.Count; i++)
        //{
        //    PlayerPrefs.SetInt("houses" + level + i.ToString(), houses[i].unlocked ? 1 : 0);
        //    for (int j = 0; j < houses[i].colors.Length; j++)
        //    {
        //        PlayerPrefs.SetInt("houses" + level + i + "_ColorCount" + j, houses[i].colors[j].count);
        //    }
        //}
    }

    public void ToggleSettings()
    {
        if (settingPanel.activeInHierarchy)
        {
            settingPanel.SetActive(false);
            UIEngaged = false;
        }
        else
        {
            settingPanel.SetActive(true);
            settingPanel.transform.GetChild(0).DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear).From(Vector3.zero);
            UIEngaged = true;
        }
    }
    public void ToggleSound()
    {
        sound = sound == 0 ? 1 : 0;
        PlayerPrefs.SetInt("sound", sound);
        soundImg.SetActive(sound == 0);
        FindObjectOfType<AudioListener>().enabled = (sound == 1);
        ToggleSettings();
    }
    public void ToggleHaptic()
    {
        haptic = haptic == 0 ? 1 : 0;
        PlayerPrefs.SetInt("haptic", haptic);
        hapticImg.SetActive(haptic == 0);
        ToggleSettings();
    }
    public void HapticManager(hapticTypes types)
    {
        if (haptic == 1)
        {
            switch (types)
            {
                case hapticTypes.soft:
                    MMVibrationManager.Haptic(HapticTypes.SoftImpact);
                    break;
                case hapticTypes.success:
                    MMVibrationManager.Haptic(HapticTypes.Success);
                    break;
                case hapticTypes.medium:
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                    break;
            }
        }
    }
}