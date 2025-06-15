using UnityEngine;

/// <summary>
/// プレイヤーが装備しているパーツと能力を設定するスクリプト
/// </summary>

public class PlayerCustomizer : MonoBehaviour
{
    // シングルトンの取得
    [SerializeField] private PlayerParts playerParts;
    [SerializeField] private PlayerStatus playerStatus;

    // データファイルの取得
    [SerializeField] private PlayerStatusData statusData;

    // プレイヤーの能力値を初期状態にリセットする
    private void resetStatus() {
        playerStatus.MoveSpeed = statusData.NormalMoveSpeed;
        playerStatus.CanUnlock = statusData.NormalCanUnlock;
        playerStatus.JumpForce = statusData.NormalJumpForce;
        playerStatus.CanPushHeavyObject = statusData.NormalCanPushHeavyObject;
    }
}
