using UnityEngine;

/// <summary>
/// インベントリのデータを保持するシングルトン
/// </summary>
public class InventoryData : MonoBehaviour
{
    public static InventoryData Instance { get; private set; }

    // 研究報告書の取得状態
    private bool playerReportObtained;
    private bool theifReportObtained;
    private bool muscleReportObtained;
    private bool fireReportObtained;
    private bool assassinReportObtained;

    // 日記の取得状態
    private bool playerDiaryObtained;
    private bool theifDiaryObtained;
    private bool muscleDiaryObtained;
    private bool fireDiaryObtained;
    private bool assassinDiaryObtained;

    private void Awake()
    {
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

    // 研究報告書の取得状態プロパティ
    public bool PlayerReportObtained { get => playerReportObtained; set => playerReportObtained = value; }
    public bool TheifReportObtained { get => theifReportObtained; set => theifReportObtained = value; }
    public bool MuscleReportObtained { get => muscleReportObtained; set => muscleReportObtained = value; }
    public bool FireReportObtained { get => fireReportObtained; set => fireReportObtained = value; }
    public bool AssassinReportObtained { get => assassinReportObtained; set => assassinReportObtained = value; }

    // 日記の取得状態プロパティ
    public bool PlayerDiaryObtained { get => playerDiaryObtained; set => playerDiaryObtained = value; }
    public bool TheifDiaryObtained { get => theifDiaryObtained; set => theifDiaryObtained = value; }
    public bool MuscleDiaryObtained { get => muscleDiaryObtained; set => muscleDiaryObtained = value; }
    public bool FireDiaryObtained { get => fireDiaryObtained; set => fireDiaryObtained = value; }
    public bool AssassinDiaryObtained { get => assassinDiaryObtained; set => assassinDiaryObtained = value; }

    // アイテム取得メソッド    
    public void SetPlayerReportObtained(bool obtained) => PlayerReportObtained = obtained;
    public void SetTheifReportObtained(bool obtained) => TheifReportObtained = obtained;
    public void SetMuscleReportObtained(bool obtained) => MuscleReportObtained = obtained;
    public void SetFireReportObtained(bool obtained) => FireReportObtained = obtained;
    public void SetAssassinReportObtained(bool obtained) => AssassinReportObtained = obtained;
    public void SetPlayerDiaryObtained(bool obtained) => PlayerDiaryObtained = obtained;
    public void SetTheifDiaryObtained(bool obtained) => TheifDiaryObtained = obtained;
    public void SetMuscleDiaryObtained(bool obtained) => MuscleDiaryObtained = obtained;
    public void SetFireDiaryObtained(bool obtained) => FireDiaryObtained = obtained;
    public void SetAssassinDiaryObtained(bool obtained) => AssassinDiaryObtained = obtained;

    // すべてのアイテムをリセットするメソッド
    public void ResetAllItems()
    {
        playerReportObtained = false;
        theifReportObtained = false;
        muscleReportObtained = false;
        fireReportObtained = false;
        assassinReportObtained = false;
        playerDiaryObtained = false;
        theifDiaryObtained = false;
        muscleDiaryObtained = false;
        fireDiaryObtained = false;
        assassinDiaryObtained = false;
    }

    /*
    // 取得済みアイテム数を取得するメソッド 
    public int GetObtainedReportsCount()
    {
        int count = 0;
        if (playerReportObtained) count++;
        if (theifReportObtained) count++;
        if (muscleReportObtained) count++;
        if (fireReportObtained) count++;
        if (assassinReportObtained) count++;
        return count;
    }

    public int GetObtainedDiariesCount()
    {
        int count = 0;
        if (playerDiaryObtained) count++;
        if (theifDiaryObtained) count++;
        if (muscleDiaryObtained) count++;
        if (fireDiaryObtained) count++;
        if (assassinDiaryObtained) count++;
        return count;
    }

    public int GetTotalObtainedItemsCount()
    {
        return GetObtainedReportsCount() + GetObtainedDiariesCount();
    }
    */
}
