using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// 生成と再利用
public class ObjectPooler
{
    private GameObject[] pool;
    private Transform parent;

    public ObjectPooler(GameObject[] initialPool, Transform parent)
    {
        pool = initialPool;
        this.parent = parent;
    }

    public void Spawn(GameObject prefab, Vector3 pos)
    {
        foreach (var obj in pool)
        {
            if (obj == null) continue;
            if (!obj.activeSelf && obj.name == prefab.name)
            {
                obj.SetActive(true);
                obj.transform.position = pos;
                return;
            }
        }

        int len = pool.Length;
        Array.Resize(ref pool, len + 1);
        pool[len] = UnityEngine.Object.Instantiate(prefab, pos, Quaternion.identity, parent);
    }
}
