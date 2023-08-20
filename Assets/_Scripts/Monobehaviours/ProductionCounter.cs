using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Game.Managers;
using Game.Tools;

public class ProductionCounter : Counter
{
    private List<DisplayCounter> m_DisplayCounters = new List<DisplayCounter>();
    private PoolManager m_PoolManager => PoolManager.Instance;

    private List<UnityAction> m_ProductionCycles = new List<UnityAction>();
    private bool m_Producing;

    private void OnValidate()
    {
        SetReferences();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        DisplayCounter.OnDisplayCounterUnlocked += addCounters;
        OnStockEmpty += Produce;
    }
    private void OnDisable()
    {
        DisplayCounter.OnDisplayCounterUnlocked -= addCounters;
        OnStockEmpty -= Produce;
    }

    private void addCounters(DisplayCounter i_DisplayCounter)
    {
        if(ItemType == i_DisplayCounter.ItemType)
        {
            m_DisplayCounters.Add(i_DisplayCounter);
            m_ProductionCycles.Add(() => Produce(i_DisplayCounter));
            if (!m_Producing)
                m_ProductionCycles[0].Invoke();
        }
    }

    private void Produce(DisplayCounter i_DisplayCounter)
    {
        if (m_ItemStack.Count(x => x.Index == i_DisplayCounter.Index) > 0)
            return;
        if (m_Producing)
        {
            m_ProductionCycles.Add(() => Produce(i_DisplayCounter));
            return;
        }

        m_Producing = true;
        int capacity = i_DisplayCounter.Capacity;
        for (int i = 0; i < capacity; i++)
        {
            this.DelayedAction(() =>
            {
                Item item = m_PoolManager.Dequeue(PoolType.Item, transform.position + new Vector3(-5, 1, 0), Quaternion.identity).GetComponent<Item>();
                ItemIn(item);
                item.ItemType = i_DisplayCounter.ItemType;
                item.Index = i_DisplayCounter.Index;
                item.OnObjectSpawn();
            }, 0.1f * i);
        }
        this.DelayedAction(() =>
        {
            m_ProductionCycles.RemoveAt(0);
            m_Producing = false;
            if (m_ProductionCycles.Count > 0)
                m_ProductionCycles[0].Invoke();

        }, 0.1f * capacity);
    }
}
