using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Game.Tools;
using Game.Data;

namespace Game.Managers
{

    public class CustomersManager : Singleton<CustomersManager>
    {
        public Transform[] SpawnPoints;
        public List<DisplayCounter> DisplayCounters => m_DisplayCounters;
        public List<DisplayProp> DisplayProps => m_DisplayProps;
        public List<CashCounter> CashCounters => m_CashCounters;

        private PoolManager m_Pool => PoolManager.Instance;

        private List<DisplayCounter> m_DisplayCounters = new List<DisplayCounter>();
        private List<DisplayProp> m_DisplayProps = new List<DisplayProp>();
        private List<CashCounter> m_CashCounters = new List<CashCounter>();

        public List<Customer> m_Customers = new List<Customer>();
        private List<Counter> m_RequiredCounter = new List<Counter>();
        private Counter m_DummyCounter;
        private NavMeshSurface m_NavMeshSurface;

        protected override void Awake()
        {
            base.Awake();
            m_NavMeshSurface = FindObjectOfType<NavMeshSurface>();
        }
        private void OnEnable()
        {
            DisplayCounter.OnDisplayCounterUnlocked += addDisplayCounters;
            DisplayProp.OnDisplayPropUnlocked += addDisplayProps;
            CashCounter.OnCashCounterUnlocked += addCashCounters;
        }
        private void OnDisable()
        {
            DisplayCounter.OnDisplayCounterUnlocked -= addDisplayCounters;
            DisplayProp.OnDisplayPropUnlocked -= addDisplayProps;
            CashCounter.OnCashCounterUnlocked -= addCashCounters;
        }

        void Start()
        {
            InvokeRepeating(nameof(spawnCustomers), 2, 2);

        }

        public void addDisplayCounters(DisplayCounter displayCounter)
        {
            m_DisplayCounters.Add(displayCounter);
            this.DelayedAction(() => m_NavMeshSurface.BuildNavMesh(), 0.2f);

        }
        private void addDisplayProps(DisplayProp displayProp)
        {
            m_DisplayProps.Add(displayProp);
            this.DelayedAction(() => m_NavMeshSurface.BuildNavMesh(), 0.2f);
        }
        private void addCashCounters(CashCounter cashCounter)
        {
            m_CashCounters.Add(cashCounter);
            this.DelayedAction(() =>
            {
                m_NavMeshSurface.BuildNavMesh();
                for (int i = 0; i < m_Customers.Count; i++)
                {
                    m_Customers[i].calulateTarget();
                }
            }, 0.2f);

        }

        private void spawnCustomers()
        {
            if (m_Customers.Count < GameConfig.CustomerSettings.CustomersPerCounter * m_DisplayCounters.Count)
            {
                m_Customers.Add(m_Pool.Dequeue(PoolType.Customer, GetRandomSpawnPoint()).GetComponent<Customer>());
            }
        }

        Vector3 GetRandomSpawnPoint()
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
                m_CashCounters = m_CashCounters.OrderBy(x => x.m_Customers.Count).ToList();
                result = m_CashCounters[0];
                return true;
            }
            else
                return false;
        }

        public void HasExit(Customer customer)
        {
            m_Customers.Remove(customer);
            m_Pool.Enqueue(PoolType.Customer, customer.gameObject);
        }
    }
}