using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerationOfStages : MonoBehaviour
{
   // public automaticFloor automaticFloor;

   [SerializeField] public AutomaticFloor automaticFloor;
    public Vector3 direction = new Vector3();//レイの方向
    public float FiringPosition = 80f;//レイの発射位置のz座標
    public float distance = 10f;//レイの距離
    float timer = 0f;//クールタイム用タイマー
    public float cooldownTime = 0.5f;//
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
        timer = cooldownTime;
    
        automaticFloor.Generate();
       // automaticFloor.GenerationOfStages();
    }
    bool IsInvokingGenerationOfStages()
    {
        Vector3 FIRINGOSITION = this.transform.position;
        FIRINGOSITION.z += FiringPosition;//レイの発射位置
        Ray ray = new Ray(FIRINGOSITION, direction);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 5);
        if (Physics.Raycast(ray, out hit, distance)) { 
            return true;
        }
        ;
        return false;
    }
}
