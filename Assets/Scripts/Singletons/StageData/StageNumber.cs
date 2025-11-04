using UnityEngine;

public class FloorNumber : MonoBehaviour
{
    // シングルトンインスタンス
    public static FloorNumber Instance { get; private set; }

    // 現在の階数
    private int currentFloor = 1;

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 階数を取得
    public int GetCurrentFloor()
    {
        return currentFloor;
    }

    // 階数を設定
    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }
}

public class StageNumber : MonoBehaviour
{
    // シングルトンインスタンス
    public static StageNumber Instance { get; private set; }

    // 現在のステージ番号
    private int currentStage = 1;

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ステージ番号を取得
    public int GetCurrentStage()
    {
        return currentStage;
    }

    // ステージ番号を設定
    public void SetCurrentStage(int stage)
    {
        currentStage = stage;
    }
}
