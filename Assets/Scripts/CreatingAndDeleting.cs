using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CreatingAndDeletingStages : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
     void OnTriggerEnter(Collider other)
     {
        if (other.tag == "ChibaCorgi") return;
        if (this.tag == "Finish")
        {
            Debug.Log("すり抜けた！");
            return;
        }
        
        other.gameObject.SetActive(false);
      
     }
}
