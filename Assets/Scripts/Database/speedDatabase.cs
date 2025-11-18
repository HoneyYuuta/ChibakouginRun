using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Speed Database", menuName = "Database/Speed Database")]

public class speedDatabase : ScriptableObject
{

    [SerializeField] private List<speedData> speedList = new List<speedData>();


    public float GetSpeedForLevel(int level)
    {
        //レベルがリストの範囲を超えないように安全に丸める
        int clampedLevel = Mathf.Clamp(level, 0, speedList.Count - 1);

        if (speedList.Count == 0)
        {
            Debug.LogWarning("Speed Databaseのリストが空です！");
            return 0f;
        }

        return speedList[clampedLevel].Speed;
    }

    //設定されている最大レベルを取得する (レベルは0から始まるので、要素数-1)
    public int GetMaxLevel()
    {
        return speedList.Count - 1;
    }
}
