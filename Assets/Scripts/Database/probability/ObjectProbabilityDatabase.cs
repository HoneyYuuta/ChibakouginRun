using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectProbabilityDatabase : ScriptableObject
{
    public List<ObjectProbability> ItemList = new List<ObjectProbability>();
}
