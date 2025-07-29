using UnityEngine;
using DG.Tweening;

public class LaserTarget : MonoBehaviour
{
    [SerializeField] private Transform laserStand;
    [SerializeField] private Transform underStand;
    
    [Header("アニメーション設定")]
    [SerializeField] private float moveDuration = 2.0f;
    [SerializeField] private float delayBetweenMovement = 0.5f;
    [SerializeField] private Ease easeType = Ease.InOutQuad;
    [SerializeField] private bool startFromTop = true;
    
    [Header("位置調整")]
    [SerializeField] private float targetHeight = 1.0f;
    
    private Sequence moveSequence;
    private float topPosition;
    private float bottomPosition;
    
    void Start()
    {
        // 初期設定を行う
        Initialize();
        
        // 初期状態でもアニメーションを開始する
        if (laserStand != null && underStand != null)
        {
            StartAnimation();
        }
    }
    
    // オブジェクトがアクティブになるたびに呼ばれる
    void OnEnable()
    {
        // Start()が既に実行済みかチェック
        if (gameObject.activeInHierarchy && Time.timeSinceLevelLoad > 0.1f)
        {
            // アクティブになったらアニメーションを開始
            if (laserStand != null && underStand != null)
            {
                StartAnimation();
            }
        }
    }
    
    void OnDisable()
    {
        // 非アクティブ時にアニメーションを停止
        if (moveSequence != null)
        {
            moveSequence.Kill();
            moveSequence = null;
        }
    }
    
    void OnDestroy()
    {
        // シーンが切り替わる際などにTweenをクリーンアップ
        moveSequence?.Kill();
    }
    
    // 初期化処理
    private void Initialize()
    {
        if (laserStand == null || underStand == null)
        {
            Debug.LogError("LaserStand または UnderStand が設定されていません");
            return;
        }

        // Targetの高さを取得（スプライトの場合）
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            targetHeight = renderer.bounds.size.y;
        }
        
        // 上下の位置を計算
        topPosition = laserStand.position.y - (targetHeight / 2);
        bottomPosition = underStand.position.y + (targetHeight / 2);
    }
    
    // アニメーションを開始
    private void StartAnimation()
    {
        // 開始位置を設定
        if (startFromTop)
        {
            transform.position = new Vector3(transform.position.x, topPosition, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, bottomPosition, transform.position.z);
        }
        
        // 移動シーケンスを作成
        CreateMovementSequence(topPosition, bottomPosition);
    }
    
    // 外部からアニメーションを再開するためのメソッド
    public void RestartTarget()
    {
        gameObject.SetActive(true);
        // OnEnableが呼ばれ、アニメーションが開始される
    }

    private void CreateMovementSequence(float topPosition, float bottomPosition)
    {
        moveSequence = DOTween.Sequence();

        if (startFromTop)
        {
            // 上から下へ、下から上へのループを作成
            moveSequence.Append(transform.DOMoveY(bottomPosition, moveDuration).SetEase(easeType))
                        .AppendInterval(delayBetweenMovement)
                        .Append(transform.DOMoveY(topPosition, moveDuration).SetEase(easeType))
                        .AppendInterval(delayBetweenMovement);
        }
        else
        {
            // 下から上へ、上から下へのループを作成
            moveSequence.Append(transform.DOMoveY(topPosition, moveDuration).SetEase(easeType))
                        .AppendInterval(delayBetweenMovement)
                        .Append(transform.DOMoveY(bottomPosition, moveDuration).SetEase(easeType))
                        .AppendInterval(delayBetweenMovement);
        }

        // 無限ループ設定
        moveSequence.SetLoops(-1, LoopType.Restart);
    }
    
    // アニメーションを停止してターゲットを非表示にする
    public void StopTarget()
    {
        // アニメーションを停止
        if (moveSequence != null)
        {
            moveSequence.Kill();
            moveSequence = null;
        }
        
        // ターゲットを非表示にする
        gameObject.SetActive(false);
        
        Debug.Log("LaserTarget: ターゲットを停止して非表示にしました");
    }
}

