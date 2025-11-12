using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class automaticFloor : MonoBehaviour
{
    public GameObject floor;
    public GameObject Obstacles;
    public GameObject Items;
    public GameObject[] floorObject;
    public int XPos=1;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            movingObject(floor, new Vector2(0, 0));
            XPos++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GenerationOfStages();
        }
    }
    public void GenerationOfStages() {
        AutomaticFloor();
        AutomaticItems();
        XPos++;
    }
    void summonObject( GameObject Object, Vector2 pox) { 
       int Pos = floorObject.Length;
        GameObject ball = Instantiate(Object, new Vector3(pox.y, pox.x, XPos * 10), Quaternion.identity);
        Array.Resize(ref floorObject, Pos+1);
        floorObject[Pos] = ball;
        ball.transform.parent = this.transform;
    }
    void movingObject(GameObject Object, Vector2 pox)
    {
        foreach (GameObject item in floorObject)
        {
            if (item == null) continue;
            if(item.tag != Object.tag) continue;
            if (item.activeSelf != false) continue;
            item.SetActive(true);
            item.transform.position = new Vector3(pox.y, pox.x, XPos * 10);
            if (item.GetComponent<MeshRenderer>().material
                == Object.GetComponent<MeshRenderer>().material) return;
            item.GetComponent<MeshRenderer>().material = Object.GetComponent<MeshRenderer>().material;
            return;
        }
        summonObject( Object,pox);

    }
    void AutomaticFloor() {
        movingObject(floor, new Vector2(0, 0));
    

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
        movingObject(Obstacles, new Vector2(1,Y));


    }
    void AutomaticPowerUpItems(int Y)
    {
        movingObject(Obstacles, new Vector2(1, Y));
    }
}
