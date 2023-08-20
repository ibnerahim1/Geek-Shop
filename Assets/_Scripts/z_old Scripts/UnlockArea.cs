//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using TMPro;
//using UnityEngine.UI;

//public class UnlockArea : MonoBehaviour
//{
//    public bool unlocked;
//    public int unlockCost, cashPaid, unlocksEncountered;
//    public GameObject[] disableObjects, enableObjects;
//    public TextMeshProUGUI cashText;
//    public Image fill;

//    private GameManager gManager;

//    // Start is called before the first frame update
//    void Start()
//    {
//        gManager = FindObjectOfType<GameManager>();
//        if (unlocked)
//        {
//            Unlock();
//        }
//        else
//        {
//            fill.fillAmount = (cashPaid / (float)unlockCost);
//            cashText.text = (unlockCost - cashPaid).ToString();
//            for (int i = 0; i < disableObjects.Length; i++)
//            {
//                disableObjects[i].SetActive(true);
//            }
//            for (int i = 0; i < enableObjects.Length; i++)
//            {
//                if (enableObjects[i].GetComponent<UnlockArea>())
//                {
//                    StartCoroutine(DisableUnlocks(enableObjects[i]));
//                    unlocksEncountered++;
//                }
//                else
//                    enableObjects[i].SetActive(false);
//            }
//            gManager.GetComponent<NavMeshSurface>().BuildNavMesh();
//        }
//    }
//    public void PayCash()
//    {
//        cashPaid+= Mathf.Min(gManager.cash, Mathf.CeilToInt(unlockCost * 0.01f));
//        gManager.cash-= Mathf.Min(gManager.cash, Mathf.CeilToInt(unlockCost * 0.01f));
//        fill.fillAmount = (cashPaid / (float)unlockCost);
//        cashText.text = (unlockCost - cashPaid).ToString();
//        if (cashPaid >= unlockCost)
//            Invoke("Unlock", 0.5f);
//    }

//    public void Unlock()
//    {
//        unlocked = true;

//        gManager.HapticManager(GameManager.hapticTypes.medium);
//        for (int i = 0; i < disableObjects.Length; i++)
//        {
//            disableObjects[i].SetActive(false);
//        }
//        for (int i = 0; i < enableObjects.Length; i++)
//        {
//            enableObjects[i].SetActive(true);
//        }
//        gManager.GetComponent<NavMeshSurface>().BuildNavMesh();
//    }

//    private IEnumerator DisableUnlocks(GameObject obj)
//    {
//        yield return new WaitForFixedUpdate();
//        if (obj.GetComponent<UnlockArea>().unlocksEncountered > 0)
//        {
//            StartCoroutine(DisableUnlocks(obj));
//        }
//        else
//        {
//            obj.SetActive(false);
//            unlocksEncountered--;
//        }
//    }
//}