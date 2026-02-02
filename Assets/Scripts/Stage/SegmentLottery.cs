using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//抽選クラス
public class SegmentLottery
{
   

    public SegmentType LastSegment { get; private set; }

    public SegmentType Draw(WeightedItem[] items)
    {
        int total = 0;
        foreach (var i in items) total += i.weight;

        int r = Random.Range(0, total);
        int current = 0;

        foreach (var i in items)
        {
            current += i.weight;
            if (r < current)
            {
                LastSegment = i.segmentType;
                return i.segmentType;
            }
        }
        throw new System.Exception("抽選失敗");
    }

}