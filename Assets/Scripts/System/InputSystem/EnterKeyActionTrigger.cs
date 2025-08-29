using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// Enteキーで発生するイベントを呼び出すスクリプト
/// </summary>
public class ObjectInteractionTrigger : MonoBehaviour
{
    // マップに落ちているパーツオブジェクトのタグ
    [SerializeField] private string partsTag = "Parts";
    // PlartsManagerの参照
    [SerializeField] private PartsManager partsManager;
    // プレイヤーの参照
    [SerializeField] private GameObject player;
    // 錆びたレバーのタグ
    [SerializeField] private string leverTag = "RustyLever";
    // ストップボタンのタグ
    [SerializeField] private string stopButtonTag = "StopButton";
    // 水タンク用のタグ
    [SerializeField] private string waterTankTag = "WaterTank";
    // 燃え盛る炎のタグ
    [SerializeField] private string burningFireTag = "BurningFire";
    
    // 接触しているコライダー
    private Collider2D touchingCollision = null;
    // デバッグ用
    [SerializeField] private bool showDebugLogs = false;

    // クラス変数として追加
    private bool isInteracting = false;
    private float lastInteractionTime = 0f;
    private float interactionCooldown = 0.5f; // クールダウン時間（秒）

    // Unityの初期化処理
    private void Start()
    {
        // 起動時に周囲のオブジェクトを確認
        CheckSurroundingObjects();
    }
    
    // オブジェクトに干渉する or ナイフを投げるメソッドを実行
    public void OnEnterKeyAction()
    {
        // 現在時刻を取得
        float currentTime = Time.time;
        
        // 1. 実行中チェック
        if (isInteracting)
        {
            if (showDebugLogs) Debug.Log("インタラクション実行中のため無視");
            return;
        }
        
        // 2. クールダウンチェック
        if (currentTime - lastInteractionTime < interactionCooldown)
        {
            if (showDebugLogs) Debug.Log("クールダウン中のため無視");
            return;
        }
        
        // フラグをセットして実行開始
        isInteracting = true;
        lastInteractionTime = currentTime;
        
        try
        {
            // 以下、元のインタラクションコード
            bool interacted = false;
            
            // 何かと接触していれば、そのオブジェクトとインタラクションを試みる
            if (touchingCollision != null)
            {
                if (touchingCollision.gameObject.CompareTag(partsTag))
                {
                    var component = touchingCollision.gameObject.GetComponent<MapParts>();
                    if (component != null)
                    {
                        partsManager.ExchangeParts(component);
                        interacted = true;
                        if (showDebugLogs) Debug.Log("パーツとインタラクトしました");
                    }
                }
                else if (touchingCollision.gameObject.CompareTag(leverTag))
                {
                    var component = touchingCollision.gameObject.GetComponent<RustyLever>();
                    if (component != null)
                    {
                        component.RotateLever();
                        interacted = true;
                        if (showDebugLogs) Debug.Log("レバーとインタラクトしました");
                    }
                }
                else if (touchingCollision.gameObject.CompareTag(stopButtonTag))
                {
                    var component = touchingCollision.gameObject.GetComponent<StopButton>();
                    if (component != null)
                    {
                        component.PressButton();
                        interacted = true;
                        if (showDebugLogs) Debug.Log("ストップボタンとインタラクトしました");
                    }
                }
                else if (touchingCollision.gameObject.CompareTag(waterTankTag))
                {
                    var component = touchingCollision.gameObject.GetComponent<WaterTank>();
                    if (component != null)
                    {
                        component.ChargeWater();
                        interacted = true;
                        if (showDebugLogs) Debug.Log("水タンクとインタラクトしました");
                    }
                }
                else if (touchingCollision.gameObject.CompareTag(burningFireTag))
                {
                    var component = touchingCollision.gameObject.GetComponent<BurningFireCheckZone>();
                    if (component != null)
                    {
                        component.FireExtinguished();
                        interacted = true;
                        if (showDebugLogs) Debug.Log("燃え盛る炎とインタラクトしました");
                    }
                }
            }

            // インタラクションがなかった場合、ナイフを投げる
            if (!interacted)
            {
                if (showDebugLogs) Debug.Log("インタラクションなし: ナイフを投げます");
                // Playerオブジェクトからナイフを投げるコンポーネントを取得と実行
                ThrowKnifeController throwKnife = player.GetComponent<ThrowKnifeController>();
                // プレイヤーのThrowKnifeメソッドを呼び出す
                throwKnife.ThrowKnife();
            }
        }
        finally
        {
            // 処理完了後、フラグをリセット
            isInteracting = false;
        }
    }
    
    // 起動時に周囲のオブジェクトをチェック
    private void CheckSurroundingObjects()
    {
        // プレイヤーの周囲にあるインタラクト可能なオブジェクトを検出
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag(partsTag) || 
                collider.gameObject.CompareTag(leverTag) || 
                collider.gameObject.CompareTag(stopButtonTag) ||
                collider.gameObject.CompareTag(waterTankTag)||
                collider.gameObject.CompareTag(burningFireTag))
            {
                // すでに接触しているオブジェクトを設定
                touchingCollision = collider;
                //if (showDebugLogs) Debug.Log($"起動時に検出: {collider.gameObject.name}");
                break;
            }
        }
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(partsTag) || 
            collision.gameObject.CompareTag(leverTag) || 
            collision.gameObject.CompareTag(stopButtonTag) ||
            collision.gameObject.CompareTag(waterTankTag) ||
            collision.gameObject.CompareTag(burningFireTag))
        {
            touchingCollision = collision;
            //if (showDebugLogs) Debug.Log($"トリガーエンター: {collision.gameObject.name}");
        }
    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (touchingCollision == collision)
        {
            touchingCollision = null;
            //if (showDebugLogs) Debug.Log($"トリガー退出: {collision.gameObject.name}");
        }
    }

    // コライダーのデバッグ描画
    private void OnDrawGizmos()
    {
        if (showDebugLogs)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
    }
}
