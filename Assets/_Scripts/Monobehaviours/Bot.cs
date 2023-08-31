//using UnityEngine;
//using UnityEngine.AI;
//using Game.Interfaces;
//using Game.Managers;
//using Game.Data;

//public class Bot : Character
//{
//    #region Public Properties
//    #region Interface Properties

//    #endregion
//    #endregion

//    #region Private Properties
//    #region References

//    #endregion
//    #endregion

//    #region Init

//    #endregion

//    #region Specific Methods

//    #endregion

//    #region Interface Methods

//    #endregion

//    #region Physics

//    #endregion


//    private StorageManager m_StorageManager => StorageManager.Instance;
//    private int m_CurrentSpeedLevel => m_StorageManager.GameData.WorkerSpeedLevel[BotIndex];
//    private int m_CurrentStackLevel => m_StorageManager.GameData.WorkerStackLevel[BotIndex];
//    public int BotIndex;

//    private NavMeshAgent m_NavMeshAgent;
//    private NavMeshHit m_NavMeshHit;
//    private Vector3 m_Destination;

//    protected override void Start()
//    {
//        base.Start();

//        m_NavMeshAgent = GetComponent<NavMeshAgent>();

//        m_NavMeshAgent.obstacleAvoidanceType = Random.Range(0, 2) == 0 ? ObstacleAvoidanceType.LowQualityObstacleAvoidance : ObstacleAvoidanceType.MedQualityObstacleAvoidance;
//        m_NavMeshAgent.avoidancePriority = Random.Range(30, 50);

//        m_NavMeshAgent.SetDestination(new Vector3(10, 0, -5));
//        NavMesh.CalculatePath(transform.position, new Vector3(10, 0, -5), NavMesh.AllAreas, m_NavMeshAgent.path);
//        if (m_NavMeshAgent.path.corners.Length > 1)
//        {
//            for (int i = 0; i < m_NavMeshAgent.path.corners.Length - 1; i++)
//            {
//                Debug.DrawLine(m_NavMeshAgent.path.corners[i], m_NavMeshAgent.path.corners[i + 1], Color.blue);
//            }
//        }
//        if (!NavMesh.SamplePosition(m_Destination, out m_NavMeshHit, 1f, NavMesh.AllAreas))
//        {
//            //m_Destination = 
//        }
//    }

//    protected override void Update()
//    {
//        base.Update();

//        if (m_CurrentCounter != null && Time.time > StackTimer + 0.1f)
//        {
//            StackTimer = Time.time;

//            atCounter();
//        }
//    }

//    private void FixedUpdate()
//    {
//        m_Rigidbody.velocity = Vector3.zero;
//        m_Rigidbody.angularVelocity = Vector3.zero;


//        if (m_CharacterState == CharacterState.Moving)
//        {
//            m_Rigidbody.velocity = transform.forward * GameConfig.WorkerSettings.WorkerSpeed[m_CurrentSpeedLevel];
//        }
//    }

//    private void startMoving()
//    {
//        ChangeState(CharacterState.Moving);
//    }
//    private void stopMoving()
//    {
//        ChangeState(CharacterState.Idle);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag(nameof(eTags.Interactable)))
//        {
//            if (m_CurrentCounter != null)
//                exitPreviousCounter();
//            m_CurrentCounter = other.GetComponent<Counter>();
//            StackTimer = 0;
//        }
//    }
//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag(nameof(eTags.Interactable)))
//        {
//            if (m_CurrentCounter != null)
//                exitPreviousCounter();
//        }
//    }

//    private void exitPreviousCounter()
//    {
//        m_CurrentCounter = null;
//    }

//    private void atCounter()
//    {
//        if (m_CurrentCounter.GetComponent<CashCounter>())
//        {
//            if (m_CurrentCounter.GetComponent<CashCounter>().IsUnlocked())
//            {
//                Pickup();
//            }
//        }
//        if (m_CurrentCounter.GetComponent<DisplayCounter>())
//        {
//            if (m_CurrentCounter.GetComponent<DisplayCounter>().IsUnlocked())
//            {
//                Drop();
//            }
//        }
//        if (m_CurrentCounter.GetComponent<ProductionCounter>())
//        {
//            Pickup();
//        }
//    }

//    private void Pickup()
//    {
//        if (m_ItemStack.Count < GameConfig.WorkerSettings.StackLimit[m_CurrentStackLevel])
//        {
//            if (m_CurrentCounter.ItemOut(out var item))
//            {
//                ItemIn(item);
//            }
//        }
//    }
//    private void Drop()
//    {
//        TryDropItems(m_CurrentCounter.GetComponent<IItemStackable>());
//    }
//}
