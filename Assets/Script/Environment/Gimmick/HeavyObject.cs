using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// プレイヤーが重いものを押すためのスクリプト
/// </summary>
public class HeavyObject : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private PlayerStatus playerStatus;

    [SerializeField, Range(0.1f, 10f)]
    private float maxSpeed = 5f; // 最大速度を制限

    private float distanceThreshold; // 離れすぎないようにする距離
    private float distanceThresholdPlus = 0.6f; // プレイヤーとの距離の閾値に追加する値
    private bool isPlayerTouching = false;
    private bool isPushing = false;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private Vector2 previousPlayerPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            previousPlayerPosition = player.transform.position;
        }

        // オブジェクトの横の長さを取得してdistanceThresholdを設定
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            distanceThreshold = spriteRenderer.bounds.size.x / 2f + distanceThresholdPlus; // 横の長さの半分に少し余裕を持たせる
        }
        else
        {
            // SpriteRendererがない場合は、デフォルト値を設定
            distanceThreshold = 0.5f;
            Debug.LogWarning("SpriteRendererが見つかりませんでした。distanceThresholdをデフォルト値に設定します。");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player != null && collision.gameObject == player)
        {
            isPlayerTouching = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isPlayerTouching) return;
        if (player != null && collision.gameObject == player && playerStatus != null && playerStatus.CanPushHeavyObject)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            PushObject();
        }
        else
        {
            StopPushing();
        }
    }

    private void PushObject()
    {
        isPushing = true;
        MoveLoop().Forget(); // 非同期ループを開始
    }

    private void StopPushing()
    {
        isPushing = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (player != null && collision.gameObject == player)
        {
            isPlayerTouching = false;
            StopPushing();
            Debug.Log("Player has stopped touching the heavy object.");
        }
    }
    private async UniTaskVoid MoveLoop()
    {
        while (isPushing)
        {
            if (rb != null && playerRb != null)
            {
                // プレイヤーとの距離を計算
                float distance = Vector2.Distance(rb.position, playerRb.position);

                // 離れすぎている場合は速度を調整
                if (distance > distanceThreshold)
                {
                    // HeavyObjectの速度を0にする
                    rb.linearVelocity = Vector2.zero;
                }
                else
                {
                    // プレイヤーの速度を直接適用（ただし最大速度を超えないように制限）
                    rb.linearVelocity = Vector2.ClampMagnitude(playerRb.linearVelocity, maxSpeed);
                }

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }
    }
}
