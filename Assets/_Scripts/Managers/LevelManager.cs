using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Game.Tools;
using Game.Data;

namespace Game.Managers
{

    public class LevelManager : Singleton<LevelManager>
    {
        public static List<Customer> Customers = new List<Customer>();
        public Transform[] SpawnPoints;

        private static NavMeshSurface m_NavMeshSurface;
        private PoolManager m_Pool => PoolManager.Instance;

        protected override void Awake()
        {
            base.Awake();
            m_NavMeshSurface = FindObjectOfType<NavMeshSurface>();
        }

        void Start()
        {
            InvokeRepeating(nameof(spawnCustomers), 2, 2);
        }

        #region Specific

        public static void BuildNavMesh()
        {
            m_NavMeshSurface.BuildNavMesh();
            for (int i = 0; i < Customers.Count; i++)
            {
                Customers[i].calulateTarget();
            }
        }
        #endregion

        #region Get Methods

        private Vector3 getRandomSpawnPoint()
        {
            if (SpawnPoints.Length == 0)
            {
                Debug.LogError("No spawn points assigned for customers!");
                return Vector3.zero;
            }

            return SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
        }
        public bool GetCashCounter(out CashCounter result)
        {
            result = null;
            if (CashCounter.Counters.Count > 0)
            {
                CashCounter.Counters = CashCounter.Counters.OrderBy(x => x.m_Customers.Count).ToList();
                result = CashCounter.Counters[0];
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Customer Pooling Handler

        private void spawnCustomers()
        {
            if (Customers.Count < GameConfig.CustomerSettings.CustomersPerCounter * (DisplayCounter.Counters.Count + DisplayProp.Counters.Count))
            {
                Customers.Add(m_Pool.Dequeue(PoolType.Customer, getRandomSpawnPoint()).GetComponent<Customer>());
            }
        }
        public void HasExit(Customer customer)
        {
            Customers.Remove(customer);
            m_Pool.Enqueue(PoolType.Customer, customer.gameObject);
        }
        #endregion
    }
}