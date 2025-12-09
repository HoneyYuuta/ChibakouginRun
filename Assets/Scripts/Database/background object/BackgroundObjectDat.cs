using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BackgroundObjectDat : ScriptableObject
{
    public List<PocketBackgroundObject> BackgroundObject = new List<PocketBackgroundObject>();
}
