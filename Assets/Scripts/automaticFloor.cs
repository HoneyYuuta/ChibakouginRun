using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class automaticFloor : MonoBehaviour
{
    public GameObject[] floorObject;
    public GameObject[] ObstaclesObject;
    public GameObject[] ItemsObject;
    public int XPos;
    // Start is called before the first frame update
    void Start()
    {
        XPos = floorObject.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AutomaticFloor();
            AutomaticItems();
            XPos++;
        }
    }

    void AutomaticFloor() {
        foreach (GameObject item in floorObject)
        {
            if(item == null) continue;
            if(item.activeSelf != false) continue;
            item.SetActive(true);
            item.transform.position = new Vector3(XPos * 10, 0, 0);
            AutomaticObstacles(XPos);
            return;
        }
        GameObject ball = Instantiate(floorObject[1], new Vector3(XPos * 10, 0, 0), Quaternion.identity);
        Array.Resize(ref floorObject, XPos+1);
        floorObject[XPos] = ball;
        ball.transform.parent = this.transform;
       

    }
    void AutomaticItems() {
        int number = (int)UnityEngine.Random.Range(1f,3f);
        int Y = 3 * (int)UnityEngine.Random.Range(-2f, 2f);
        Debug.Log(number);
        if (number ==1) {
            AutomaticObstacles(Y);
        }
        else {
            AutomaticPowerUpItems(Y);
        }

    }


    void AutomaticObstacles(int Y) {
        foreach (GameObject item in ObstaclesObject)
        {
            if (item == null) continue;
            if (item.activeSelf != false) continue;
            item.SetActive(true);
            item.transform.position = new Vector3(XPos * 10, 1, Y);
            return;
        }
        GameObject ball = Instantiate(ObstaclesObject[1], new Vector3(XPos * 10, 1, Y), Quaternion.identity);
        Array.Resize(ref ObstaclesObject, XPos + 1);
        ObstaclesObject[XPos] = ball;
        ball.transform.parent = this.transform;
      
    }
    void AutomaticPowerUpItems(int Y)
    {
        foreach (GameObject item in ItemsObject)
        {
            if (item == null) continue;
            if (item.activeSelf != false) continue;
            item.SetActive(true);
            item.transform.position = new Vector3(XPos * 10, 1, Y);
            return;
        }
        GameObject ball = Instantiate(ItemsObject[1], new Vector3(XPos * 10, 1, Y), Quaternion.identity);
        Array.Resize(ref ItemsObject, XPos + 1);
        ItemsObject[XPos] = ball;
        ball.transform.parent = this.transform;
    }
}
