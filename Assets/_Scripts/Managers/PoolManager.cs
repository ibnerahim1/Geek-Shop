using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Game.Tools;
using Game.Interfaces;

namespace Game.Managers
{

    public class PoolManager : Singleton<PoolManager>
    {
        public List<PoolableItem> PoolType;

        public Dictionary<ePoolType, Queue<GameObject>> m_Pool = new Dictionary<ePoolType, Queue<GameObject>>();
        private GameObject m_Prefab;
        private GameObject d_Prefab;

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < PoolType.Count; i++)
            {
                CreatePool(PoolType[i].PoolType, PoolType[i].Prefab, PoolType[i].InitialPoolSize);
            }
        }

        public void CreatePool(ePoolType i_PoolType, GameObject i_Prefab, int i_InitialSize)
        {
            m_Pool[i_PoolType] = new Queue<GameObject>();

            for (int i = 0; i < i_InitialSize; i++)
            {
                d_Prefab = Instantiate(i_Prefab);
                d_Prefab.gameObject.SetActive(false);
                m_Pool[i_PoolType].Enqueue(d_Prefab);
            }
        }

        public GameObject Dequeue(ePoolType i_PoolType, Vector3 i_Position = default, Quaternion i_Rotation = default, Transform i_Parent = default)
        {
            if (!m_Pool.ContainsKey(i_PoolType))
            {
                Debug.LogWarning($"WARNING :\tpool type {i_PoolType} not found");
                return null;
            }
            if (m_Pool[i_PoolType].Count < 1)
            {
                CreatePool(i_PoolType, PoolType.FirstOrDefault(item => item.PoolType == i_PoolType).Prefab, 1);
            }
            if (m_Pool.Count == 0)
            {
                d_Prefab = Instantiate(m_Prefab);
            }
            d_Prefab = m_Pool[i_PoolType].Dequeue();
            d_Prefab.SetActive(true);

            d_Prefab.transform.position = i_Position;
            d_Prefab.transform.rotation = i_Rotation;
            d_Prefab.transform.parent = i_Parent;

            d_Prefab.GetComponent<IPoolable>().OnObjectSpawn();

            return d_Prefab;
        }

        public void Enqueue(ePoolType i_PoolType, GameObject i_Prefab)
        {
            i_Prefab.gameObject.SetActive(false);

            if (!m_Pool.ContainsKey(i_PoolType))
            {
                Debug.LogWarning($"WARNING :\tpool type {i_PoolType} not found");
                return;
            }
            m_Pool[i_PoolType].Enqueue(i_Prefab);
            i_Prefab.GetComponent<IPoolable>().OnObjectDespawn();
        }
    }

    [System.Serializable]
    public class PoolableItem
    {
        public ePoolType PoolType;
        public GameObject Prefab;
        public int InitialPoolSize = 10;
    }
}