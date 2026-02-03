using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoBackground : MonoBehaviour
{
    public BackgroundObjectDat BackgroundObject;
    int backgroundObjectOrder;
    GameObject gameObject;
    Vector3 backgroundObjectCoordinate;
    int XCoordinate = 0;
    // Start is called before the first frame update
    void Start()
    {
        Awake();
    }
    //データベースから背景オブジェクトを取得して配置する
    void Awake()
    {
        if (BackgroundObject.BackgroundObject.Count <= backgroundObjectOrder) return;
        gameObject = BackgroundObject.BackgroundObject[backgroundObjectOrder].BackgroundObject;
        backgroundObjectCoordinate = BackgroundObject.BackgroundObject[backgroundObjectOrder].BackgroundObjectCoordinate;
        XCoordinate = BackgroundObject.BackgroundObject[backgroundObjectOrder].BackgroundObjectOrder;
        backgroundObjectOrder++;
    }

    // Instantiateを使って背景オブジェクトを配置する
    public void AutomaticBackground(int x ,float xx)
    {
        if(backgroundObjectCoordinate == null) return;
        if(XCoordinate != x) return;
        Instantiate(gameObject, new Vector3(backgroundObjectCoordinate.x, backgroundObjectCoordinate.y,((XCoordinate-1) * xx)), Quaternion.identity);
        Awake();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
