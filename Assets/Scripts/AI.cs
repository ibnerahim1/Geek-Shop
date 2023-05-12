using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public Vector3 target;

    private NavMeshAgent nvAgent;
    private Animator anim;
    private NavMeshSurface nvSurface;
    private NavMeshHit hit;
    private GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = FindObjectOfType<GameManager>();
        nvAgent = GetComponent<NavMeshAgent>();
        anim = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Animator>();
        anim.gameObject.SetActive(true);
        anim.Play("walk");
        Vector3 point = new Vector3(Random.Range(-95, 95), 0, Random.Range(5, 190));
        while (!NavMesh.SamplePosition(point, out hit, 1f, NavMesh.AllAreas))
        {
            point = new Vector3(Random.Range(-95, 95), 0, Random.Range(5, 190));
        }
        nvAgent.obstacleAvoidanceType = Random.Range(0, 2) == 0 ? ObstacleAvoidanceType.LowQualityObstacleAvoidance : ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        nvAgent.avoidancePriority = Random.Range(30, 50);
        transform.position = point;
        nvAgent.enabled = true;
        nvAgent.SetDestination(target);
        SetDestination(new Vector3(Mathf.Clamp(target.x + Random.Range(-20, 20), -95, 95), 0, Mathf.Clamp(target.z + Random.Range(-20, 20), 5, 190)));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 2)
        {
            SetDestination(new Vector3(Mathf.Clamp(target.x + Random.Range(-20, 20), -95, 95), 0, Mathf.Clamp( target.z + Random.Range(-20, 20), 5, 190)));
        }
        NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, nvAgent.path);
        //if (nvAgent.path.corners.Length > 1)
        //{
        //    for (int i = 0; i < nvAgent.path.corners.Length - 1; i++)
        //    {
        //        Debug.DrawLine(nvAgent.path.corners[i], nvAgent.path.corners[i + 1], Color.blue);
        //    }
        //}
    }
    private void SetDestination(Vector3 des)
    {
        while (!NavMesh.SamplePosition(des, out hit, 1f, NavMesh.AllAreas))
        {
            des = new Vector3(Mathf.Clamp(target.x + Random.Range(-20, 20), -95, 95), 0, Mathf.Clamp(target.z + Random.Range(-20, 20), 5, 190));
        }
        target = des;
        nvAgent.SetDestination(target);
    }
}
