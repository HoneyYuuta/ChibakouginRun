using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    // —h‚ç‚·ŠÖ”
    public void Shake()
    {
        // —h‚ê‚Ì‹­‚³: 0.5f, ŠÔ: 0.2•b
        transform.DOShakePosition(0.2f, 0.5f, 20, 90, false, true);
    }
}
