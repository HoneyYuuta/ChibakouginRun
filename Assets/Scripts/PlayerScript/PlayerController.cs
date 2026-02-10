using UnityEngine;
using UnityEngine.SceneManagement;

//プレイヤーの入力検知と各コンポーネントの統括を行うメインクラス

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerLaneMover))]
[RequireComponent(typeof(PlayerSpeedHandler))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerLaneMover laneMover;
    private PlayerSpeedHandler speedHandler;

    public float CurrentSpeed => speedHandler.CurrentSpeed;
    public float BuffTimeRatio => speedHandler.BuffTimeRatio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        laneMover = GetComponent<PlayerLaneMover>();
        speedHandler = GetComponent<PlayerSpeedHandler>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) laneMover.MoveLeft();
        else if (Input.GetKeyDown(KeyCode.D)) laneMover.MoveRight();
    }

    private void FixedUpdate()
    {
        speedHandler.UpdateSpeed(Time.fixedDeltaTime);
        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        Vector3 forwardVel = transform.forward * speedHandler.CurrentSpeed;
        forwardVel.y = rb.velocity.y;
        rb.velocity = forwardVel;
    }

    //アイテム、障害物の処理
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Items>();
        if (item != null)
        {
            item.ApplyEffect(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ChibaCorgi"))
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    //外部呼び出し用

    //加速効果を適用
    public void ApplyTemporarySpeedUp(float duration)
    {
        speedHandler.ApplyTemporarySpeedUp(duration);
    }

    //減速効果を適用
    public void ApplyTemporarySpeedDown(float duration, float penaltyRatio)
    {
        speedHandler.ApplyPercentageSpeedDown(duration, penaltyRatio);
    }

    public void IncreaseLevel()
    {
        speedHandler.IncreaseLevel();
    }

    public void DecreaseLevel()
    {
        speedHandler.DecreaseLevel();
    }

    public void StopMovement()
    {
        speedHandler.StopMovement();
    }
}