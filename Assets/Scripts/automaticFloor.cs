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
    [SerializeField]
    private ItemDataBase StageDat;
    public GameObject[] floorObject;
    public int XPos=0;
    public float ItemWidth = 1.5f;
    public int X = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            movingObject(floor, new Vector2(0, 0));
            XPos++;
        }
        StageChange();
    }

    void StageChange()
    {
        if (StageDat == null) return;
        if (StageDat.ItemList.Count <= X)X = 0;
        floor = StageDat.ItemList[X].FloorObject;
        Obstacles = StageDat.ItemList[X].ObstaclesObject;
        Items = StageDat.ItemList[X].ItemObject;
        X++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StageChange();
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
            if (item.activeSelf != false) continue;
            if (item.tag != Object.tag) continue;
            item.SetActive(true);
            item.transform.position = new Vector3(pox.y, pox.x, XPos * 10);
            if (item.GetComponent<MeshRenderer>().material
                == Object.GetComponent<MeshRenderer>().sharedMaterial) return;
            item.GetComponent<MeshRenderer>().material = Object.GetComponent<MeshRenderer>().sharedMaterial;
            if(item.GetComponent<MeshFilter>().mesh == Object.GetComponent<MeshFilter>().sharedMesh) return;
            item.GetComponent<MeshFilter>().mesh = Object.GetComponent<MeshFilter>().sharedMesh;
            return;
        }
        summonObject( Object,pox);

    }
    void AutomaticFloor() {
        movingObject(floor, new Vector2(0, 0));
    

    }
    void AutomaticItems() {
        int number = (int)UnityEngine.Random.Range(1f,11f);
        Debug.Log(number);
        if (number > 7) return;
        float Y = ItemWidth * (int)UnityEngine.Random.Range(-2f, 2f);
      
        if (number <= 4)
        {
            AutomaticObstacles(Y);
            return;
        }
        AutomaticPowerUpItems(Y);
    }
   

    void AutomaticObstacles(float Y) {
        movingObject(Obstacles, new Vector2(1,Y));
    }
    void AutomaticPowerUpItems(float Y)
    {
        movingObject(Items, new Vector2(1, Y));
    }
}
