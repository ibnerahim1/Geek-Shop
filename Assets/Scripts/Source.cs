using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Source : MonoBehaviour
{
    public Transform arrangement, car;
    public enum SourceType { cash, comics, merchs, cloths, cards}
    public SourceType resourceType;
    public int elementIndex, limit, count;
    public List<Transform> items = new List<Transform>();
    private GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < count; i++)
        {

        }
    }

    void Update()
    {
    }
}