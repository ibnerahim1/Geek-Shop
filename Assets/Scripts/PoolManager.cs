using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Transform cashObj;
    public List<Transform> cashRepository = new List<Transform>();
    public List<List<Transform>> comicsRepository = new List<List<Transform>>();
    public List<List<Transform>> merchsRepository = new List<List<Transform>>();
    public List<List<Transform>> clothsRepository = new List<List<Transform>>();
    private GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = FindObjectOfType<GameManager>();

        for (int i = 0; i < 100; i++)
        {
            Transform temp = Instantiate(cashObj, transform.GetChild(0));
            cashRepository.Add(temp);
            temp.gameObject.SetActive(false);
        }
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < gManager.comicLevel; j++)
            {
                Transform temp = Instantiate(gManager.comicsObj[gManager.comicLevel], transform.GetChild(0));
                comicsRepository[gManager.comicLevel].Add(temp);
                temp.gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < gManager.comicLevel; j++)
            {
                Transform temp = Instantiate(gManager.comicsObj[gManager.comicLevel], transform.GetChild(0));
                comicsRepository[gManager.comicLevel].Add(temp);
                temp.gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < gManager.comicLevel; j++)
            {
                Transform temp = Instantiate(gManager.comicsObj[gManager.comicLevel], transform.GetChild(0));
                comicsRepository[gManager.comicLevel].Add(temp);
                temp.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator PoolCash(Transform obj, float t)
    {
        yield return new WaitForSeconds(t);

        cashRepository.Add(obj);
        obj.gameObject.SetActive(false);
    }
}
