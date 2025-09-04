using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Collections.Generic;

/// <summary>
/// ゲーム全体のライフタイムスコープを設定するクラス
/// </summary>
public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log("GameLifetimeScope 登録開始");

        // Player, Ground, GameOverManager, PlayerStatusの登録（ここは変更なし）
        #region Singleton_Objects
        // プレイヤーを自動検索
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            builder.RegisterInstance(player);
            Debug.Log($"Playerが正常に登録されました: {player.name}");
        }
        else
        {
            Debug.LogError("Playerタグの付いたGameObjectが見つかりません");
        }

        // Groundを自動検索
        var ground = Object.FindAnyObjectByType<Ground>();
        if (ground != null)
        {
            builder.RegisterInstance(ground);
            Debug.Log($"Groundコンポーネントが正常に登録されました: {ground.name}");
        }
        else
        {
            Debug.LogError("Groundコンポーネントが見つかりません");
        }

        // GameOverManagerを自動検索
        var gameOverManager = Object.FindAnyObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            builder.RegisterInstance(gameOverManager);
            Debug.Log($"GameOverManagerが正常に登録されました: {gameOverManager.name}");
        }
        else
        {
            Debug.LogError("GameOverManagerが見つかりません");
        }

        // PlayerStatusをSingletons/PlayerDataから取得
        var playerDataObject = GameObject.Find("Singletons/PlayerData");
        if (playerDataObject != null)
        {
            var playerStatus = playerDataObject.GetComponentInChildren<PlayerStatus>();
            if (playerStatus != null)
            {
                builder.RegisterInstance(playerStatus);
                Debug.Log($"PlayerStatusが正常に登録されました: {playerStatus.name}");
            }
            else
            {
                Debug.LogError("Singletons/PlayerDataの子オブジェクトにPlayerStatusコンポーネントが見つかりません");
            }
        }
        else
        {
            Debug.LogError("Singletons/PlayerDataオブジェクトが見つかりません");
        }
        #endregion

        // HeavyObject
    var heavyObjects = Object.FindObjectsByType<HeavyObject>(FindObjectsSortMode.None);
    if (heavyObjects.Length > 0)
    {
        // 1. 他のクラスが注入できるよう、リストだけを登録する
        builder.RegisterInstance<IReadOnlyList<HeavyObject>>(heavyObjects);

        // 2. コンテナ構築後に、各インスタンスへDIを実行するよう予約する
        builder.RegisterBuildCallback(resolver =>
        {
            foreach (var heavyObject in heavyObjects)
            {
                // resolver.Inject(instance)で、既存インスタンスにDIを実行
                resolver.Inject(heavyObject);
            }
        });
        Debug.Log($"{heavyObjects.Length}個のHeavyObjectを登録 & 注入予約");
    }

    // DoorKey
    var doorKeys = Object.FindObjectsByType<DoorKey>(FindObjectsSortMode.None);
    if (doorKeys.Length > 0)
    {
        // 1. リストを登録
        builder.RegisterInstance<IReadOnlyList<DoorKey>>(doorKeys);
        // 2. 構築後にDIを実行
        builder.RegisterBuildCallback(resolver =>
        {
            foreach (var doorKey in doorKeys)
            {
                resolver.Inject(doorKey);
            }
        });
        Debug.Log($"{doorKeys.Length}個のDoorKeyを登録 & 注入予約");
    }

    // FallingCeilingScript
    var fallingCeilingScripts = Object.FindObjectsByType<FallingCeilingScript>(FindObjectsSortMode.None);
    if (fallingCeilingScripts.Length > 0)
    {
        // 1. リストを登録
        builder.RegisterInstance<IReadOnlyList<FallingCeilingScript>>(fallingCeilingScripts);
        // 2. 構築後にDIを実行
        builder.RegisterBuildCallback(resolver =>
        {
            foreach (var fallingCeiling in fallingCeilingScripts)
            {
                resolver.Inject(fallingCeiling);
            }
        });
        Debug.Log($"{fallingCeilingScripts.Length}個のFallingCeilingScriptを登録 & 注入予約");
    }
        Debug.Log("GameLifetimeScope 登録完了");
    }
}