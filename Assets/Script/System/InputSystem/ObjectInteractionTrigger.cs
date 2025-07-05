using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// マップ上のオブジェクトを判定し，それに応じたイベントを呼び出すスクリプト
/// </summary>

public class ObjectInteractionTrigger : MonoBehaviour
{
    // マップに落ちているパーツオブジェクトのタグ
    [SerializeField] private string partsTag = "Parts";
    // PlayerManagerの参照
    [SerializeField] private PartsManager partsManager;
    // 錆びたレバーのタグ
    [SerializeField] private string leverTag = "RustyLever";
    // 決定ボタンの入力アクション
    [SerializeField] private InputActionProperty interactAction;
    // 接触しているコライダー
    private Collider2D touchingCollision = null;

    // Unityの初期化処理
    private void Start()
    {
        // マップに落ちているパーツにインタラクト可能にする
        InteractParts(this.GetCancellationTokenOnDestroy()).Forget();
        InteractLever(this.GetCancellationTokenOnDestroy()).Forget();
    }


    // Interactの共通部分   
    async UniTask Interact(CancellationToken ct)
    {
        
    }

    private async UniTask Interact<T>(string tag, System.Action<T> onInteract, CancellationToken ct) where T : Component
    {
        while (true)
        {
            await interactAction.action.OnStartedAsync(ct);

            if (touchingCollision == null || !touchingCollision.gameObject.CompareTag(tag))
                continue;

            var component = touchingCollision.gameObject.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError(typeof(T).Name + "コンポーネントが見つかりません。");
                continue;
            }

            onInteract(component);
        }
    }

    // マップに落ちているパーツにインタラクトするメソッド
    private UniTask InteractParts(CancellationToken ct)
    {
        return Interact<MapParts>(partsTag, mapParts => partsManager.ExchangeParts(mapParts), ct);
    }

    // レバーにインタラクトするメソッド
    private UniTask InteractLever(CancellationToken ct)
    {
        return Interact<RustyLever>(leverTag, lever => lever.RotateLever(), ct);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        touchingCollision = collision;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        touchingCollision = null;
    }
}
