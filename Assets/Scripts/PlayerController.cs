using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField][Header("横移動速度")] private float speed;
    [SerializeField][Header("前移動速度")] private float frontSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = transform.right * speed;
        }
        // Aキー（左移動）
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = -transform.right * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
