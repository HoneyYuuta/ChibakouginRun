using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class RotatingObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       transform
       .DORotate(new Vector3(0, 360, 0), 5, RotateMode.FastBeyond360)
       .SetRelative(true)
       .SetEase(Ease.Linear)
       .SetLoops(-1);
        
    }

    // Update is called once per frame
    void Update()
    {
       

    }
}
