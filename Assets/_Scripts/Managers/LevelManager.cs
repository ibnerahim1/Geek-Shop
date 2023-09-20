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
        #region Public Properties
        public Transform[] SpawnPoints;
        public List<Customer> Customers { get; } = new List<Customer>();
        public List<DisplayCounter> DisplayCounters { get; } = new List<DisplayCounter>();
        public List<ArcadeCounter> ArcadeCounters { get; } = new List<ArcadeCounter>();
        public List<CashCounter> CashCounters { get; internal set; } = new List<CashCounter>();
        //public NavMeshSurface m_NavMeshSurface;
        #endregion


        #region Private Properties
        private PoolManager m_Pool => PoolManager.Instance;
        #endregion


        #region Init
        void Start()
        {
            InvokeRepeating(nameof(spawnCustomers), 2, 2);
        }

        #endregion

        #region Specific Methods
        public void AddCounters(DisplayCounter counter)
        {
            DisplayCounters.Add(counter);
            //BuildNavMesh();
        }
        public void AddCounters(ArcadeCounter counter)
        {
            ArcadeCounters.Add(counter);
            //BuildNavMesh();
        }
        public void AddCounters(CashCounter counter)
        {
            CashCounters.Add(counter);
            //BuildNavMesh();
        }

        //public void BuildNavMesh()
        //{
        //    this.DelayedAction(() =>
        //    {
        //        m_NavMeshSurface.BuildNavMesh();
        //        for (int i = 0; i < Customers.Count; i++)
        //        {
        //            Customers[i].calulateTarget();
        //        }
        //    }, 0.21f);
        //}
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
            if (CashCounters.Count > 0)
            {
                CashCounters = CashCounters.OrderBy(x => x.m_Customers.Count).ToList();
                result = CashCounters[0];
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Customer Pooling Handler
        private void spawnCustomers()
        {
            if (Customers.Count < GameConfig.CustomerSettings.CustomersPerCounter * (DisplayCounters.Count + ArcadeCounters.Count))
            {
                Customers.Add(m_Pool.Dequeue(ePoolType.Customer, getRandomSpawnPoint()).GetComponent<Customer>());
            }
        }
        public void HasExit(Customer customer)
        {
            Customers.Remove(customer);
            m_Pool.Enqueue(ePoolType.Customer, customer.gameObject);
        }
        #endregion
    }
}