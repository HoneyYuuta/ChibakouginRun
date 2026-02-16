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
        //上下運動
        var seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(0.5f, 1).SetRelative(true).SetEase(Ease.InOutSine))
            .Append(transform.DOMoveY(-0.5f, 1).SetRelative(true).SetEase(Ease.InOutSine))
            .SetLoops(-1);



    }

    // Update is called once per frame
    void Update()
    {
       

    }
}
