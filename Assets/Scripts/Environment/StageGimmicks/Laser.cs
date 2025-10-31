using UnityEngine;
using VContainer;
/// <summary>
/// レーザーのスクリプト
/// </summary>      

public class Laser : StoppableGimick
{
    [SerializeField] private Transform laserStand; // レーザーの台座
    [SerializeField] private GameObject laserBeam; // レーザー本体
    [Inject] private GameOverManager gameOverManager; // GameOverManagerの参照
    [Inject] private GameObject player; // プレイヤーオブジェクト
    [SerializeField] private LaserTarget target; // GameObject → LaserTargetに変更

    [SerializeField] private bool isActive = true;
    private bool isKilled = false;

    void Start()
    {
        if (!isActive) StopGimick();
    }

    void Update()
    {
        // レーザーがアクティブである場合のみ動作
        if (!isActive)
        {
            laserBeam.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーオブジェクトに当たった場合
        if (collision.gameObject == player)
        {
            gameOverManager.GameOver(); // GameOverManagerのインスタンスを使用して呼び出す
        }
    }
    public override bool IsRunning => isActive; // ギミックが動作中かどうかを示すプロパティ

    public override void StartGimick()
    {
        if (isKilled)
        {
            return;
        }
        // StopableGimmickのStartGimickメソッドをオーバーライド
        isActive = true;
        laserBeam.SetActive(true); // レーザー本体を表示する

        // ターゲットの動作を再開
        target.RestartTarget();
    }

    public override void StopGimick()
    {
        // StopableGimmickのStopGimickメソッドをオーバーライド
        isActive = false;
        laserBeam.SetActive(false); // レーザー本体を非表示にする
        target.StopTarget(); // ターゲットの動作を停止
    }

    public void KillLaser()
    {
        isKilled = true;
    }
}
