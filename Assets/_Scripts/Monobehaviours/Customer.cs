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
    #region Public Properties
    public List<Counter> TargetPoints = new List<Counter>();
    #endregion

    #region Private Properties
        #region References
        private LevelManager m_LevelManager => LevelManager.Instance;
        private PoolManager m_PoolManager => PoolManager.Instance;
        private NavMeshAgent m_NavMeshAgent;
        #endregion

    private eCustomerState m_CustomerState;
    private NavMeshHit m_NavMeshHit;
    private Vector3 m_Destination;
    private Vector3 spawnPosition;
    private int ServiceQty;
    private int m_TotalBill;
    private int m_PaidBill;
    private float routeTime;
    #endregion

    #region Init
    protected override void Start()
    {
        base.Start();

        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_NavMeshAgent.obstacleAvoidanceType = Random.Range(0, 2) == 0 ? ObstacleAvoidanceType.LowQualityObstacleAvoidance : ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        m_NavMeshAgent.avoidancePriority = Random.Range(30, 50);
    }
    #endregion

    #region Update
    protected override void Update()
    {
        base.Update();

        if(m_CharacterState == eCharacterState.Moving)
        {
            if(Vector3.Distance(transform.position, m_Destination) < m_NavMeshAgent.stoppingDistance)
            {
                arrivedAtDestination();
            }
            else if (m_CharacterState == eCharacterState.Moving && m_NavMeshAgent.path.corners.Length > 1)
            {
                transform.LookAt( new Vector3(m_NavMeshAgent.path.corners[1].x, transform.position.y, m_NavMeshAgent.path.corners[1].z));
                //transform.LookAt(Vector3.Lerp( transform.position + transform.forward, new Vector3(m_NavMeshAgent.path.corners[1].x, transform.position.y, m_NavMeshAgent.path.corners[1].z), Time.deltaTime));
            }
        }
        if (m_NavMeshAgent.path.corners.Length > 1)
        {
            for (int i = 0; i < m_NavMeshAgent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(m_NavMeshAgent.path.corners[i], m_NavMeshAgent.path.corners[i + 1], Color.blue);
            }
        }

        if(m_CurrentCounter != null)
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


        if (m_CharacterState == eCharacterState.Moving)
        {
            if(Vector3.Distance(transform.position, m_Destination) > m_NavMeshAgent.stoppingDistance)
                m_Rigidbody.velocity = transform.forward * GameConfig.CustomerSettings.CustomerSpeed;
        }
    }
    #endregion

    #region Specific Methods
    public void CreateRequiredCounters()
    {
        TargetPoints.Clear();
        List<Counter> tempRequiredCounter = new List<Counter>();
        for (int i = 0; i < m_LevelManager.DisplayCounters.Count; i++)
        {
            tempRequiredCounter.Add(m_LevelManager.DisplayCounters[i]);
        }
        for (int i = 0; i < m_LevelManager.ArcadeCounters.Count; i++)
        {
            tempRequiredCounter.Add(m_LevelManager.ArcadeCounters[i]);
        }
        for (int i = 0; i < Random.Range(0, Mathf.Min(6, tempRequiredCounter.Count)) + 1; i++)
        {
            Counter temp = tempRequiredCounter[Random.Range(0, tempRequiredCounter.Count)];
            TargetPoints.Add(temp);
            tempRequiredCounter.Remove(temp);
        }
    }

    public void calulateTarget()
    {
        switch (m_CustomerState)
        {
            case eCustomerState.Shopping:
                if (TargetPoints.Count > 0)
                {
                    SetDestination(TargetPoints[0].transform.position);
                }
                break;
            case eCustomerState.WaitingForBilling:
                if (m_LevelManager.GetCashCounter(out CashCounter result) && !m_CurrentCounter)
                {
                    m_CurrentCounter = result;
                    SetDestination(result.GetComponent<CashCounter>().GetPosition(this));
                }
                break;
            case eCustomerState.Billing:
                break;
            case eCustomerState.DoneBilling:
                break;

            default:
                break;
        }
    }
    public void SetDestination(Vector3 i_Position)
    {
        m_Destination = i_Position;
        m_NavMeshAgent.SetDestination(m_Destination);
        startMoving();
    }

    private void arrivedAtDestination()
    {
        switch (m_CustomerState)
        {
            case eCustomerState.Shopping:
                if (IsCurrentRequiredCounter(TargetPoints[0].GetComponent<Counter>()))
                {
                    ServiceQty = Random.Range(1, 4);
                    m_CurrentCounter = TargetPoints[0];
                    StackTimer = 0;
                    stopMoving();
                    if (m_CurrentCounter.CounterType == eCounterType.Display)
                        m_TotalBill += m_CurrentCounter.m_DisplayCounter.SellingPrice * ServiceQty;
                }
                break;
            case eCustomerState.WaitingForBilling:
                transform.DOLookAt(transform.position + Vector3.back, 0.1f).SetEase(Ease.Linear);
                if(m_CurrentCounter.m_CashCounter.IsFirstInQueue(this))
                {
                    StackTimer = 0;
                    SwitchCustomerState(eCustomerState.Billing);
                    m_Rigidbody.isKinematic = true;
                    stopMoving();
                }
                break;
            case eCustomerState.Billing:
                break;
            case eCustomerState.DoneBilling:
                    m_LevelManager.HasExit(this);
                break;
        }
    }

    private void startMoving()
    {
        ChangeState(eCharacterState.Moving);
    }
    private void stopMoving()
    {
        ChangeState(eCharacterState.Idle);
    }

    private bool IsCurrentRequiredCounter(Counter counter)
    {
        return TargetPoints.Count > 0 ? (counter == TargetPoints[0] ? true : false) : false;
    }
    private void exitPreviousCounter()
    {
        m_CurrentCounter = null;
    }

    private void SwitchCustomerState(eCustomerState customerState)
    {
        m_CustomerState = customerState;
    }
    #endregion

    #region Interface Methods
    public void OnObjectDespawn()
    {
        while (m_ItemStack.Count > 0)
        {
            m_PoolManager.Enqueue(ePoolType.Item, m_ItemStack.Pop().gameObject);
        }
    }

    public void OnObjectSpawn()
    {
        base.SetReferences();

        m_PaidBill = 0;
        CreateRequiredCounters();
        spawnPosition = transform.position;
        SwitchCustomerState(eCustomerState.Shopping);
        this.DelayedAction(()=>calulateTarget(), 0.1f);
    }
    #endregion

    #region Interaction Logic
    private void atCounter()
    {
        switch (m_CurrentCounter.CounterType)
        {
            case eCounterType.Display:
                if (m_CurrentCounter.m_DisplayCounter.IsUnlocked() && m_CustomerState == eCustomerState.Shopping)
                {
                    Pickup();
                }
                break;
            case eCounterType.Arcade:
                if (m_CurrentCounter.m_ArcadeCounter.IsUnlocked() && m_CustomerState == eCustomerState.Shopping)
                {
                    PlayArcade();
                }
                break;
            case eCounterType.Cash:
                if (m_CurrentCounter.m_CashCounter.IsUnlocked())
                {
                    PayBill();
                }
                break;
        }
    }

    private void Pickup()
    {
        if (ServiceQty < 1)
        {
            TargetPoints.RemoveAt(0);
            if (TargetPoints.Count < 1)
                SwitchCustomerState(eCustomerState.WaitingForBilling);
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
    private void PlayArcade()
    {

    }

    private void PayBill()
    {
        if (m_CustomerState == eCustomerState.Billing && m_CurrentCounter.m_CashCounter.CashiersNearby > 0)
        {
            if (m_PaidBill < m_TotalBill)
            {
                int value = Mathf.Min((int)(m_TotalBill / 20) + 1, m_TotalBill - m_PaidBill);
                m_PaidBill += value;
                m_CurrentCounter.m_CashCounter.PayBill(value, m_PoolManager.Dequeue(ePoolType.Cash, transform.position + Vector3.up, Quaternion.identity, m_CurrentCounter.transform).GetComponent<IItem>());
            }
            else
            {
                m_CurrentCounter.m_CashCounter.RemoveCustomer(this);
                SwitchCustomerState(eCustomerState.DoneBilling);
                m_Rigidbody.isKinematic = false;
                exitPreviousCounter();
                SetDestination(spawnPosition);
            }
        }
    }
    #endregion
}