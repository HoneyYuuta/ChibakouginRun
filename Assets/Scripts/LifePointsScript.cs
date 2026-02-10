using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifePointsScript : MonoBehaviour
{
    [SerializeField]
    Image[] HPImage;
    //アニメーション画像
    [SerializeField]
    GameObject HPAnimationImage;

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
        if (IsDead()) return;

        HP--;
        HPImage[HP].color = new Color(1, 1, 1, 0.5f);
        var seq = DOTween.Sequence();
        HPAnimationImage.transform.position = HPImage[HP].transform.position;
        HPAnimationImage.SetActive(true);
        seq.Append(HPAnimationImage.transform.DOMove(new Vector3(0, -500, 0), 1).SetRelative(true).SetEase(Ease.InSine)).
            AppendCallback(() =>
            {
                HPAnimationImage.SetActive(false);
            });




    }
    public void Heal() {
        if (HP >= HPImage.Length) return;
        HPImage[HP].color = new Color(1, 1, 1, 1);
        HP++;
    }
    public bool IsDead() {
        return HP <= 0;
    }
}
