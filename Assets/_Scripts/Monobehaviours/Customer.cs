using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using DG.Tweening;
using Game.Data;
using Game.Interfaces;
using Game.Managers;
using Game.Tools;

public class Customer : Character, IPoolable
{
    private CustomersManager m_CustomersManager => CustomersManager.Instance;
    private PoolManager m_PoolManager => PoolManager.Instance;

    public List<Counter> m_requiredCounters = new List<Counter>();
    private CustomerState m_CustomerState;

    private int ServiceQty;
    private NavMeshAgent m_NavMeshAgent;
    private NavMeshHit m_NavMeshHit;
    private Vector3 m_Destination;
    private int m_TotalBill;
    private Vector3 spawnPosition;
    private float routeTime;
    private CashCounter m_CashCounter;

    protected override void Start()
    {
        base.Start();

        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        m_NavMeshAgent.obstacleAvoidanceType = Random.Range(0, 2) == 0 ? ObstacleAvoidanceType.LowQualityObstacleAvoidance : ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        m_NavMeshAgent.avoidancePriority = Random.Range(30, 50);
    }
    protected override void Update()
    {
        base.Update();

        if (m_CharacterState == CharacterState.Moving && Vector3.Distance(transform.position, m_Destination) > 0.1f)
        {
            transform.DOLookAt(new Vector3(m_NavMeshAgent.path.corners[1].x, transform.position.y, m_NavMeshAgent.path.corners[1].z), 0.2f).SetEase(Ease.Linear);
        }
        if (m_NavMeshAgent.path.corners.Length > 1)
        {
            for (int i = 0; i < m_NavMeshAgent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(m_NavMeshAgent.path.corners[i], m_NavMeshAgent.path.corners[i + 1], Color.blue);
            }
        }

        if(m_CurrentCounter == null)
        {
            if (Time.time > routeTime + 1 && Vector3.Distance(transform.position, m_Destination) < 0.1f)
            {
                routeTime = Time.time;
                calulateTarget();
            }
        }
        else
        {
            if (Time.time > StackTimer + 0.1f)
            {
                StackTimer = Time.time;
                atCounter();
            }
        }
    }
    private void FixedUpdate()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;


        if (m_CharacterState == CharacterState.Moving)
        {
            if(Vector3.Distance(transform.position, m_Destination) > 0.1f)
                m_Rigidbody.velocity = transform.forward * GameConfig.CustomerSettings.CustomerSpeed;
        }
    }

    public void OnObjectDespawn()
    {
        while (m_ItemStack.Count > 0)
        {
            m_PoolManager.Enqueue(PoolType.Item, m_ItemStack.Pop().gameObject);
        }
    }

    public void OnObjectSpawn()
    {
        base.SetReferences();

        m_requiredCounters.Clear();
        CreateRequiredCounters();
        spawnPosition = transform.position;
        SwitchCustomerState(CustomerState.Shopping);
        this.DelayedAction(()=>calulateTarget(), 0.1f);
    }

    public void CreateRequiredCounters()
    {
        m_requiredCounters.Clear();
        List<Counter> tempRequiredCounter = new List<Counter>();
        for (int i = 0; i < m_CustomersManager.DisplayCounters.Count; i++)
        {
            tempRequiredCounter.Add(m_CustomersManager.DisplayCounters[i]);
        }
        for (int i = 0; i < m_CustomersManager.DisplayProps.Count; i++)
        {
            tempRequiredCounter.Add(m_CustomersManager.DisplayProps[i]);
        }
        for (int i = 0; i < Random.Range(0, Mathf.Min(6, tempRequiredCounter.Count)) + 1; i++)
        {
            Counter temp = tempRequiredCounter[Random.Range(0, tempRequiredCounter.Count)];
            m_requiredCounters.Add(temp);
            tempRequiredCounter.Remove(temp);
        }
    }

    public void calulateTarget()
    {
        switch (m_CustomerState)
        {
            case CustomerState.Shopping:
                if (m_requiredCounters.Count > 0)
                {
                    setDestination(m_requiredCounters[0].transform.position);
                }
                break;
            case CustomerState.WaitingForBilling:
                if(m_CustomersManager.GetCashCounter(out CashCounter result))
                {
                    m_CashCounter = result;
                    setDestination(result.GetComponent<CashCounter>().GetPosition(this));
                }
                break;
            case CustomerState.Billing:
                break;
            case CustomerState.DoneBilling:
                if(Vector3.Distance(transform.position, spawnPosition) < 0.1f)
                {
                    m_CustomersManager.HasExit(this);
                }
                else
                    setDestination(spawnPosition);
                break;

            default:
                break;
        }
    }
    private void setDestination(Vector3 i_Position)
    {
        m_Destination = i_Position;
        m_NavMeshAgent.SetDestination(m_Destination);
        startMoving();
    }

    private void startMoving()
    {
        ChangeState(CharacterState.Moving);
    }
    private void stopMoving()
    {
        ChangeState(CharacterState.Idle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(eTags.Counter)))
        {
            if(m_CustomerState == CustomerState.Shopping)
            {
                if (IsCurrentRequiredCounter(other.GetComponent<Counter>()))
                {

                    ServiceQty = Random.Range(3, 7);
                    m_CurrentCounter = other.GetComponent<Counter>();
                    StackTimer = 0;
                    stopMoving();
                }   
            }
            if(m_CustomerState == CustomerState.WaitingForBilling)
            {
                if(m_CashCounter == other.GetComponent<CashCounter>())
                {
                    m_CurrentCounter = other.GetComponent<Counter>();
                    StackTimer = 0;
                    stopMoving();
                }
            }
        }
    }

    private void SwitchCustomerState(CustomerState customerState)
    {
        m_CustomerState = customerState;
        switch (customerState)
        {
            case CustomerState.Shopping:
                break;
            case CustomerState.WaitingForBilling:
                break;
            case CustomerState.Billing:
                break;
            case CustomerState.DoneBilling:
                break;
            default:
                break;
        }
    }

    #region Specific
    private bool IsCurrentRequiredCounter(Counter counter)
    {
        return m_requiredCounters.Count > 0 ? (counter == m_requiredCounters[0] ? true : false) : false;
    }
    private void exitPreviousCounter()
    {
        m_CurrentCounter = null;
    }
    #endregion


    private void atCounter()
    {
        if (m_CurrentCounter.GetComponent<CashCounter>())
        {
            if (m_CurrentCounter.GetComponent<CashCounter>().IsUnlocked())
            {
                PayBill();
            }
        }
        else if (m_CurrentCounter.GetComponent<DisplayCounter>())
        {
            if (m_CurrentCounter.GetComponent<DisplayCounter>().IsUnlocked())
            {
                Pickup();
            }
        }
    }

    private void Pickup()
    {
        if (ServiceQty < 1)
        {
            m_requiredCounters.RemoveAt(0);
            if (m_requiredCounters.Count < 1)
                SwitchCustomerState(CustomerState.WaitingForBilling);
            exitPreviousCounter();
            calulateTarget();
            return;
        }
        if (m_CurrentCounter.ItemOut(out var item))
        {
            ItemIn(item);
            ServiceQty--;
        }
    }
    private void PayBill()
    {
        if (m_CustomerState == CustomerState.WaitingForBilling)
        {
            m_CurrentCounter.GetComponent<CashCounter>().TryToPayBill(5, m_PoolManager.Dequeue(PoolType.Cash, transform.position + Vector3.up, Quaternion.identity, m_CurrentCounter.transform).GetComponent<IItem>());
            this.DelayedAction(() => m_CashCounter.RemoveCustomer(this), 0.2f);
            SwitchCustomerState(CustomerState.DoneBilling);
            exitPreviousCounter();
            calulateTarget();
        }
    }
}