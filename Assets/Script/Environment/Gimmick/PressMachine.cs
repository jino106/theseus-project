using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;


/// <summary>
/// プレス機を動かすスクリプト。プレス機の台座にアタッチすること。
/// </summary>

public class PressMachine : StoppableGimick
{
    // Plateオブジェクト
    [SerializeField] private GameObject plate;
    // Plateの落下位置の判定用オブジェクト
    [SerializeField] private GameObject pressArea;
    // Plateの開始位置（ローカル座標）
    [SerializeField] private Vector2 posStart;
    // Plateのスタンバイ位置（ローカル座標）
    [SerializeField] private Vector2 posReady;
    // Plateの落下位置（ローカル座標）
    [SerializeField] private Vector2 posPressed;
    //プレス機のクールタイム
    [SerializeField] private float coolTime;
    // PlateオブジェクトのRigidBody2D
    private Rigidbody2D plateRigidBody;
    // 動作するかを判定
    private bool isMoving;
    // キャンセレーショントークン
    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        // オブジェクトがアタッチされているかチェック
        if (plate == null)
        {
            Debug.LogError("PressMachine: Plate object is undefined.");
        }
        if (pressArea == null)
        {
            Debug.LogError("PressMachine: PressArea is undefined");
        }
        plateRigidBody = plate.GetComponent<Rigidbody2D>();
        if (plateRigidBody == null)
        {
            Debug.LogError("PressMachine: RigidBody2D component cannot found in Plate object.");
        }
        // トークンを生成
        cancellationTokenSource = new CancellationTokenSource();
        isMoving = true;
        // プレス機のPlateを初期位置へ
        plate.transform.localPosition = posStart;
        // 動作開始
        Debug.Log("PressMachine started");
        MoveLoop(cancellationTokenSource.Token).Forget();
    }

    async UniTask MoveLoop(CancellationToken MyToken = default)
    {
        // Start関数が呼び出されるまで待つ
        await this.GetAsyncStartTrigger().StartAsync();
        // 動作のループ
        while (isMoving)
        {
            Debug.Log("---  Move begin ---");
            // Plateをスタンバイ位置へ移動
            await plateRigidBody.DOLocalPath(
                path : new Vector2[] { posStart, posReady },
                duration : 0.2f
            ).WithCancellation(MyToken);
            // スタンバイ位置へ移動したらちょっと待つ
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: MyToken);
            // Plateを落下位置へ移動
            await plateRigidBody.DOLocalPath(
                path: new Vector2[] { posReady, posPressed },
                duration: 1.0f
            ).WithCancellation(MyToken);
            // 落下位置へ移動したらちょっと待つ
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: MyToken);
            // Plateを再びスタート位置へ移動
            await plateRigidBody.DOLocalPath(
                path: new Vector2[] { posPressed, posStart },
                duration: 4.0f
            ).WithCancellation(MyToken);
            // 指定されたクールタイム分だけ待つ
            await UniTask.Delay(TimeSpan.FromSeconds(coolTime), cancellationToken: MyToken);
            Debug.Log("--- Move end ---");
        }
    }

    // プレス機の動作を止める関数
    public override void StopGimick()
    {
        // 非同期処理をキャンセル
        cancellationTokenSource.Cancel();
        isMoving = false;
        Debug.Log("Stopped PressMachine!");
    }

    // オブジェクトが破棄される時に非同期処理をキャンセルする
    private void OnDestroy()
    {
        // 非同期処理をキャンセル
        cancellationTokenSource.Cancel();
    }
}
