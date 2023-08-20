using UnityEngine;
using System.Collections.Generic;
using Game.Tools;

namespace Game.Managers
{

    public class QuestManager : Singleton<QuestManager>
    {
        public List<Quest> activeQuests;
        // Add references to other quest-related data

        // Method to add a new quest to the activeQuests list
        public void AddQuest(Quest quest)
        {
            activeQuests.Add(quest);
        }

        // Method to remove a completed quest from the activeQuests list
        public void CompleteQuest(Quest quest)
        {
            activeQuests.Remove(quest);
            // Implement reward and completion logic here
        }
    }
    [System.Serializable]
    public class Quest
    {
        public string questName;
        public string description;
        public int requiredPotions;
        // Add more properties like rewards, progress tracking, etc.
    }
}