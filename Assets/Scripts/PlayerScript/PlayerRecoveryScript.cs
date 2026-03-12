using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoveryScript : MonoBehaviour
{
    [SerializeField] LifePointsScript lifePointsScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   //3D貫通回復アイテムがプレイヤーに当たったとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "recoveryItems")
        {
            lifePointsScript.Heal();
            Destroy(other.gameObject);
        }
    }
}
