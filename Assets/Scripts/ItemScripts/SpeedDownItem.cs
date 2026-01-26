using UnityEngine;

public class SpeedDownItem : MonoBehaviour, Items
{
    [Header("効果時間（秒）")]
    [SerializeField] private float duration = 3.0f;

    [Header("速度減少率 (0.1 = 10%ダウン)")]
    [SerializeField, Range(0f, 1f)] private float penaltyRatio = 0.1f;

    public void ApplyEffect(GameObject target)
    {
        // AudioSourceコンポーネントを取得
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }

        PlayerController player = target.GetComponent<PlayerController>();

        if (player != null)
        {
            //減少率も一緒に渡す
            player.ApplyTemporarySpeedDown(duration, penaltyRatio);
        }

        CameraShake.Instance.Shake();

        Destroy(gameObject);
    }
}