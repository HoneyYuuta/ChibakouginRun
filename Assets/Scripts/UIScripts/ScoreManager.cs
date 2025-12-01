using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    //GameUIManagerから読めるようにpublicプロパティにする
    public float TotalScore { get; private set; } = 0;

    private float bonusScore = 0;

    [SerializeField] private GameObject PlayerObj;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Update()
    {
        if (PlayerObj == null) return;

        // スコア計算だけを行う
        float distanceScore = PlayerObj.transform.position.z;
        TotalScore = distanceScore + bonusScore;

    }


    public void AddScore(int amount)
    {
        bonusScore += amount;
        Debug.Log("Bonus Added! Current Bonus: " + bonusScore);
    }
}