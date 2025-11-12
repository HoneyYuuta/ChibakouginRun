using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibaCorgiController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField][Header("前移動速度")] private float frontSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("ChibaCorgiController: Rigidbody がアタッチされていません。");
            enabled = false;
        }
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        // 常に前進する速度（重力などのY成分は保持）
        Vector3 forwardVel = transform.forward * frontSpeed;
        forwardVel.y = rb.velocity.y;
        rb.velocity = forwardVel;
    }
}
