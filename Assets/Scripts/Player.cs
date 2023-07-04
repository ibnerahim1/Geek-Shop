using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;
//using MoreMountains.NiceVibrations;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public bool bot;
    public Transform hand;
    public float moveSpeed = 1, collectionTime;
    public int stackLimit;
    public List<Transform> stack;

    private GameManager gManager;
    private PoolManager pool;
    private Animator anim;
    private bool moving;
    private float time;
    private Camera cam;
    private Vector3 mousePos, mousePos1, offset;
    private Transform temp;
    private Rigidbody rb;

    void Start()
    {
        cam = Camera.main;
        gManager = FindObjectOfType<GameManager>();
        pool = FindObjectOfType<PoolManager>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (bot)
        {

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            moving = gManager.gameStarted && Input.GetMouseButton(0) && !gManager.UIEngaged;
            if (moving)
            {
                mousePos1 = cam.ScreenToViewportPoint(Input.mousePosition);
                offset = (mousePos1 - mousePos) * 100;
                if (offset.magnitude > 0.01f)
                    transform.DOLookAt(transform.position + cam.transform.right * offset.x + new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z) * offset.y, 0.1f).SetEase(Ease.Linear);
            }
            anim.SetFloat("moving", moving && offset.magnitude > 0.01f ? moveSpeed : 0);
        }
    }
    private void FixedUpdate()
    {
        if (!bot)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (moving && offset.magnitude > 0.01f)
            {
                rb.velocity = transform.forward * moveSpeed;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("unlock") && !Input.GetMouseButton(0) && !bot)
        {
            if (gManager.cash > 0)
            {
                UnlockArea unlock = other.GetComponent<UnlockArea>();
                if (time < Time.timeSinceLevelLoad - collectionTime && unlock.unlockCost > unlock.cashPaid)
                {
                    time = Time.timeSinceLevelLoad;
                    if (pool.cashRepository.Count > 0)
                    {
                        temp = pool.cashRepository[0];
                        pool.cashRepository.Remove(temp);
                        temp.gameObject.SetActive(true);
                        temp.transform.position = transform.position + Vector3.up * 0.5f;
                    }
                    else
                        temp = Instantiate(pool.cashObj, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                    temp.DOJump(other.transform.position, 1.5f, 1, 0.5f).SetEase(Ease.Linear);
                    StartCoroutine(pool.PoolCash(temp, 0.5f));
                    unlock.PayCash();
                    gManager.soundfx.PlayOneShot(gManager.unstackingClip);
                    gManager.HapticManager(GameManager.hapticTypes.soft);
                    gManager.UpdateUI();
                }
            }
        }
        if (other.CompareTag("source"))
        {

            //if(house.indices.Contains(colorIndex) && time < Time.timeSinceLevelLoad - 0.01f && blocks.Count > 0 && !house.unlocked)
            //{
            //    time = Time.timeSinceLevelLoad;
            //    Transform block = blocks[blocks.Count - 1];
            //    house.AddColor(block, colorIndex);
            //    block.parent = gManager.transform.GetChild(0);
            //    blocks.Remove(block);
            //    gManager.soundfx.PlayOneShot(gManager.unstackingClip);
            //    gManager.HapticManager(GameManager.hapticTypes.soft);
            //    gManager.blocksRepository.Add(block);
            //    gManager.UpdateUI();
            //}
        }
    }
}