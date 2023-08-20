using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Game.Managers;
using Game.Tools;
using Game.Data;

public class ProductionCounter : Counter
{
    public static Dictionary<ItemType, ProductionCounter> Counters { get; internal set; } = new Dictionary<ItemType, ProductionCounter>();

    private PoolManager m_PoolManager => PoolManager.Instance;

    private List<int> m_ProductionCycles = new List<int>();

    private bool m_Producing;

    private void OnValidate()
    {
        SetReferences();
    }
    private void Start()
    {
        if (Counters.ContainsKey(ItemType))
            Counters[ItemType] = this;
        else
            Counters.Add(ItemType, this);
    }

    public static void Produce(ItemType i_Type, int i_Index)
    {
        Counters[i_Type].Produce(i_Index);
    }

    private void Produce(int i_Index)
    {
        if (m_ItemStack.Count(x => x.Index == i_Index) > 0)
            return;
        if (m_Producing)
        {
            if (!m_ProductionCycles.Contains(i_Index))
                m_ProductionCycles.Add(i_Index);
            return;
        }

        m_Producing = true;
        for (int i = 0; i < GameConfig.CounterSettings.ProductionQTY; i++)
        {
            this.DelayedAction(() =>
            {
                Item item = m_PoolManager.Dequeue(PoolType.Item, transform.position + new Vector3(-5, 1, 0), Quaternion.identity).GetComponent<Item>();
                ItemIn(item);
                item.ItemType = ItemType;
                item.Index = i_Index;
                item.OnObjectSpawn();
            }, GameConfig.CounterSettings.ProductionSpeed * i);
        }
        this.DelayedAction(() =>
        {
            if (m_ProductionCycles.Contains(i_Index))
                m_ProductionCycles.Remove(i_Index);
            if (m_ProductionCycles.Count > 0)
                Produce(m_ProductionCycles[0]);
            m_Producing = false;
        }, GameConfig.CounterSettings.ProductionSpeed * GameConfig.CounterSettings.ProductionQTY);
    }
}
