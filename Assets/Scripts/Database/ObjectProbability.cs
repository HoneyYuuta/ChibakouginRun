using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ObjectProbability
{
    [Header("障害物の確率")]
    [Range(0, 100)] public int FrequencyOfObstacles = 40;//障害物の確率
    [Header("アイテムの確率")]
    [Range(0, 100)] public int FrequencyOfItems = 30;//アイテムの確率
    [Header("2つの障害物の確率")]
    [Range(0, 100)]
    public int ProbabilityOf2Obstacles = 0;//2つの障害物の確率
}
