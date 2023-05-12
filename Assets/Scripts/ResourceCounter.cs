using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResourceCounter : MonoBehaviour
{
    public enum ResourceType { cash, comics, merchs, cloths, cards}
    public ResourceType resourceType;
    public ResourceData[] resourceObj;
    public int elementIndex, limit;
    public float productionTime;
    public enum SpawnType { linear, array }
    public SpawnType spawnType;
    public List<Transform> elements;
    public Transform arrangement;
    private float time;
    private GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        elements = new List<Transform>();
        gManager = FindObjectOfType<GameManager>();
        if (limit > arrangement.childCount)
            limit = arrangement.childCount;
    }

    void Update()
    {
        //if (unlocked && blocks.Count < limit && time < Time.timeSinceLevelLoad - productionTime)
        //{
        //    time = Time.timeSinceLevelLoad;
        //    if (gManager.blocksRepository.Count > 5)
        //    {
        //        Transform block = gManager.blocksRepository[0];
        //        blocks.Add(block);
        //        gManager.blocksRepository.Remove(block);
        //        block.parent = transform;
        //        block.position = transform.position + Vector3.up * 0.1f * blocks.Count;
        //        block.eulerAngles = new Vector3(0, Random.Range(-5, 5), 0);
        //        block.localScale = Vector3.one;
        //    }
        //    else
        //        blocks.Add(Instantiate(block, transform.position + Vector3.up * 0.1f * blocks.Count, Quaternion.Euler(0, Random.Range(-10, 10), 0), transform));
        //    blocks[blocks.Count - 1].GetComponent<MeshRenderer>().material.color = gManager.colors[colorIndex];
        //}
    }

    public void SpawnElements()
    {
        if(spawnType == SpawnType.linear)
        {
        }
        else if(spawnType == SpawnType.array)
        {

        }
    }
    private void ElementSpawner()
    {
        switch (resourceType)
        {
            case ResourceType.cash:
                elements.Add(Instantiate(resourceObj[0].elementsObj[0], arrangement.GetChild(elements.Count).position, arrangement.GetChild(elements.Count).rotation, arrangement.GetChild(elements.Count)));
                break;
            case ResourceType.comics:
                elements.Add(Instantiate(resourceObj[1].elementsObj[Random.Range(0, resourceObj[1].elementsObj.Length)], arrangement.GetChild(elements.Count).position, arrangement.GetChild(elements.Count).rotation, arrangement.GetChild(elements.Count)));
                break;
            case ResourceType.merchs:
                elements.Add(Instantiate(resourceObj[2].elementsObj[Random.Range(0, resourceObj[2].elementsObj.Length)], arrangement.GetChild(elements.Count).position, arrangement.GetChild(elements.Count).rotation, arrangement.GetChild(elements.Count)));
                break;
            case ResourceType.cloths:
                elements.Add(Instantiate(resourceObj[3].elementsObj[Random.Range(0, resourceObj[3].elementsObj.Length)], arrangement.GetChild(elements.Count).position, arrangement.GetChild(elements.Count).rotation, arrangement.GetChild(elements.Count)));
                break;
            case ResourceType.cards:
                break;
        }
    }

}
[System.Serializable]
public class ResourceData
{
    public Transform[] elementsObj;
}
