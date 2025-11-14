using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GenerationOfStages : MonoBehaviour
{
    public automaticFloor automaticFloor;
    public Vector3 v3 = new Vector3();
    public float distance = 10f;
   float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer > 0) return;
        if (IsInvokingGenerationOfStages()) return;
            timer = 0.5f;
            automaticFloor.GenerationOfStages(); 
    }
    bool IsInvokingGenerationOfStages()
    {
        Ray ray = new Ray(this.transform.position, v3);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 5);
        if (Physics.Raycast(ray, out hit, distance)) return true;
        return false;
    }
}
