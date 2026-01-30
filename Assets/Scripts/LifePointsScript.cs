using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePointsScript : MonoBehaviour
{
    [SerializeField]
    Image[] HPImage;
    [SerializeField]
    public int HP = 5;
    // Start is called before the first frame update
    void Start()
    {
        for (int h = 0; h < HPImage.Length; h++)
        {
            if (h >= HP) HPImage[h].color = new Color(1, 1, 1, 0.5f);
            else HPImage[h].color = new Color(1, 1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
      //if (Input.GetKeyDown(KeyCode.D)) {
      //      Damage();
      //  }
      //  if (Input.GetKeyDown(KeyCode.H)) {
      //      Heal();
      //  }
    }
    public void Damage() {
        if (HP <= 0) return;
        HP--;
        HPImage[HP].color = new Color(1, 1, 1, 0.5f);
        

    }
    public void Heal() {
        if (HP >= HPImage.Length) return;
        HPImage[HP].color = new Color(1, 1, 1, 1);
        HP++;
    }
}
