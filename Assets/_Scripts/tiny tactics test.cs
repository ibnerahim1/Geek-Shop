//using UnityEngine;
//using System.Collections.Generic;

//public class InventorySystem : MonoBehaviour
//{
//    [Min(0)]
//    private int m_PremiumBalance = 100;
//    [Min(0)]
//    private int m_StandardBalance = 50;

//    private Dictionary<EPremiumTypes, int> m_PremiumItems = new Dictionary<EPremiumTypes, int>();       // here the value is cost of Item
//    private Dictionary<EStandardTypes, int> m_StandardItems = new Dictionary<EStandardTypes, int>();    // here the value is cost of Item

//    private List<EPremiumTypes> m_ConsumableItems = new List<EPremiumTypes>();

//    public void Initialize()
//    {
//        m_PremiumItems.Add(EPremiumTypes.HealUp, 20);
//        m_PremiumItems.Add(EPremiumTypes.Immune, 15);
//        m_PremiumItems.Add(EPremiumTypes.Troop, 30);
//        m_PremiumItems.Add(EPremiumTypes.AirStrike, 35);
//        //Add more Premium Items;

//        m_StandardItems.Add(EStandardTypes.Sword, 15);
//        m_StandardItems.Add(EStandardTypes.Shield, 10);
//        m_StandardItems.Add(EStandardTypes.Fence, 20);
//        //Add more Premium Items;
//    }

//    public void BuyPremiumItem(EPremiumTypes i_Type)
//    {
//        if(m_PremiumBalance >= m_PremiumItems[i_Type])
//        {
//            m_ConsumableItems.Add(i_Type);
//            m_PremiumBalance -= m_PremiumItems[i_Type];
//        }
//    }

//    public void BuildStandardItem(EStandardTypes i_Type)
//    {
//        if (m_StandardBalance >= m_StandardItems[i_Type])
//        {
//            m_StandardBalance -= m_StandardItems[i_Type];
//            switch (i_Type)
//            {
//                case EStandardTypes.Sword:
//                    buildBasicSword();
//                    break;
//                case EStandardTypes.Shield:
//                    buildBasicShield();
//                    break;
//                case EStandardTypes.Fence:
//                    buildBasicFence();
//                    break;
//            }
//        }
//    }

//    public void ConsumePremiumItem(EPremiumTypes i_Type)
//    {
//        if (m_ConsumableItems.Contains(i_Type))
//        {
//            m_ConsumableItems.Remove(i_Type);

//            switch (i_Type)
//            {
//                case EPremiumTypes.HealUp:
//                    consumeHealUp();
//                    break;
//                case EPremiumTypes.Immune:
//                    beImmune();
//                    break;
//                case EPremiumTypes.Troop:
//                    spawnTroops();
//                    break;
//                case EPremiumTypes.AirStrike:
//                    doAirStrike();
//                    break;
//            }
//        }
//    }


//    #region Building Items

//    private void buildBasicSword()
//    {
//        //logic for building sword
//    }
//    private void buildBasicShield()
//    {
//        //logic for building shield
//    }
//    private void buildBasicFence()
//    {
//        //logic for building fence
//    }
//    #endregion

//    #region Consume Items

//    private void consumeHealUp()
//    {
//        //logic for Healing up
//    }
//    private void beImmune()
//    {
//        //logic for damage Immunity
//    }
//    private void spawnTroops()
//    {
//        //logic for spawning Troops
//    }
//    private void doAirStrike()
//    {
//        //logic for AirStrike
//    }
//    #endregion
//}

//public enum EPremiumTypes
//{
//    HealUp,
//    Immune,
//    Troop,
//    AirStrike,
//    //Add more Standard Elements
//}
//public enum EStandardTypes
//{
//    Sword,
//    Shield,
//    Fence
//    //Add more Standard Elements
//}






//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChallengeSystem : MonoBehaviour
//{
//    [Serializable]
//    public class Challenge
//    {
//        public string Id;
//        [TextArea]
//        public string Description;
//        public int Reward;
//    }

//    public List<Challenge> ChallengeList = new List<Challenge>();

//    private Challenge m_CurrentChallenge = null;
//    private DateTime m_ChallengeCompleteTime;
//    private DateTime m_ChallengeAssignTime;
//    private int m_RewardRecieved;

//    private float m_Tick;

//    private void Update()
//    {

//        if (m_CurrentChallenge != null)
//        {
//            if(Time.time > m_Tick + 0.1f) //Assignment Completion check delay
//            {
//                m_Tick = Time.time;
//                if (checkAssignmentCompletion())
//                    completeCurrentChallenge();

//            }

//            TimeSpan temp_TimeSinceChallengeAssignment = DateTime.Now - m_ChallengeAssignTime;

//            if (temp_TimeSinceChallengeAssignment.TotalHours >= 24)
//            {
//                assignRandomChallenge();
//            }
//        }
//        else
//        {
//            TimeSpan temp_TimeSinceChallengeComplete = DateTime.Now - m_ChallengeCompleteTime;

//            if (temp_TimeSinceChallengeComplete.TotalHours >= 2)
//            {
//                assignRandomChallenge();
//            }
//        }
//    }

//    private bool checkAssignmentCompletion()
//    {
//        // logic for checking if the challenge is complete and return result
//        return true;
//    }

//    private void completeCurrentChallenge()
//    {
//        if (m_CurrentChallenge != null)
//        {
//            m_RewardRecieved += m_CurrentChallenge.Reward;
//            m_CurrentChallenge = null;
//            m_ChallengeCompleteTime = DateTime.Now;
//            m_ChallengeAssignTime = m_ChallengeCompleteTime.AddHours(2);
//        }
//    }

//    private void assignRandomChallenge()
//    {
//        if (ChallengeList.Count > 0)
//        {
//            m_CurrentChallenge = ChallengeList[UnityEngine.Random.Range(0, ChallengeList.Count)];
//            ChallengeList.Remove(m_CurrentChallenge);
//            m_ChallengeAssignTime = DateTime.Now;
//        }
//        else
//        {
//            Debug.Log("No challenges left in the pool.");
//        }
//    }
//}