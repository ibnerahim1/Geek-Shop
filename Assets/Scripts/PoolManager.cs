using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Transform cashObj, comicsObj, merchsObj, clothsObj;
    public List<Transform> cashRepository = new List<Transform>();
    public List<Transform> comicsRepository = new List<Transform>();
    public List<Transform> merchsRepository = new List<Transform>();
    public List<Transform> clothsRepository = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Transform temp = Instantiate(cashObj, transform.GetChild(0));
            cashRepository.Add(temp);
            temp.gameObject.SetActive(false);
        }
        //for (int i = 0; i < 50; i++)
        //{
        //    Transform temp = Instantiate(comicsObj, transform.GetChild(0));
        //    comicsRepository.Add(temp);
        //    temp.gameObject.SetActive(false);
        //}
        //for (int i = 0; i < 50; i++)
        //{
        //    Transform temp = Instantiate(merchsObj, transform.GetChild(0));
        //    merchsRepository.Add(temp);
        //    temp.gameObject.SetActive(false);
        //}
        //for (int i = 0; i < 50; i++)
        //{
        //    Transform temp = Instantiate(clothsObj, transform.GetChild(0));
        //    clothsRepository.Add(temp);
        //    temp.gameObject.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
