using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VContainer;

/// <summary>
/// プレイヤーのパーツ占有率を管理するクラス
/// </summary>
public class PlayerPartsRatio : MonoBehaviour
{
    [Inject] private PlayerParts playerParts;
    
    private Dictionary<PartsOwnerType, float> partsRatios = new Dictionary<PartsOwnerType, float>();

    private void Start()
    {
        CalculatePartsRatio();
    }

    public void CalculatePartsRatio()
    {
        partsRatios.Clear();

        List<PartsOwnerType> allParts = new List<PartsOwnerType>
        {
            (PartsOwnerType)playerParts.LeftArm,
            (PartsOwnerType)playerParts.RightArm,
            (PartsOwnerType)playerParts.LeftLeg,
            (PartsOwnerType)playerParts.RightLeg
        };

        var partsCounts = allParts.GroupBy(p => p)
                                   .ToDictionary(g => g.Key, g => g.Count());

        foreach (var parts in partsCounts)
        {
            float ratio = (parts.Value / 4f) * 100f;
            partsRatios[parts.Key] = ratio;
        }

        LogPartsRatio();
    }

    public float GetPartsRatio(PartsOwnerType ownerType)
    {
        return partsRatios.ContainsKey(ownerType) ? partsRatios[ownerType] : 0f;
    }

    public PartsOwnerType GetDominantParts()
    {
        if (partsRatios.Count == 0) return PartsOwnerType.Player;
        return partsRatios.OrderByDescending(x => x.Value).First().Key;
    }

    public PartsRatioState GetPartsRatioState(PartsOwnerType ownerType)
    {
        float ratio = GetPartsRatio(ownerType);

        if (ratio >= 100f)
            return PartsRatioState.Full;
        else if (ratio >= 75f)
            return PartsRatioState.ThreeQuarters;
        else if (ratio >= 50f)
            return PartsRatioState.Half;
        else if (ratio >= 25f)
            return PartsRatioState.Quarter;
        else
            return PartsRatioState.None;
    }

    public List<PartsOwnerType> GetPartsAboveRatio(float threshold)
    {
        return partsRatios.Where(x => x.Value >= threshold)
                          .Select(x => x.Key)
                          .ToList();
    }

    public Dictionary<PartsOwnerType, float> GetAllRatios()
    {
        return new Dictionary<PartsOwnerType, float>(partsRatios);
    }

    public bool HasFullRatioParts()
    {
        return partsRatios.Any(x => x.Value >= 100f);
    }

    public PartsOwnerType GetFullRatioParts()
    {
        var fullParts = partsRatios.FirstOrDefault(x => x.Value >= 100f);
        return fullParts.Key;
    }

    private void LogPartsRatio()
    {
        Debug.Log("=== パーツ占有率 ===");
        foreach (var ratio in partsRatios)
        {
            PartsRatioState state = GetPartsRatioState(ratio.Key);
            Debug.Log($"{ratio.Key}: {ratio.Value}% ({state})");
        }
    }

    public bool IsAllQuarters()
    {
        return
            Mathf.Approximately(GetPartsRatio(PartsOwnerType.Player), 25f) &&
            Mathf.Approximately(GetPartsRatio(PartsOwnerType.Thief), 25f) &&
            Mathf.Approximately(GetPartsRatio(PartsOwnerType.Muscle), 25f) &&
            Mathf.Approximately(GetPartsRatio(PartsOwnerType.Fire), 25f) &&
            Mathf.Approximately(GetPartsRatio(PartsOwnerType.Assassin), 25f);
    }
}

/// <summary>
/// パーツ占有率の状態
/// </summary>
public enum PartsRatioState
{
    None,
    Quarter,
    Half,
    ThreeQuarters,
    Full
}
