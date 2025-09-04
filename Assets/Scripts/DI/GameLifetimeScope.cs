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

        // Singletons/PlayerDataから取得するオブジェクト
        var playerDataObject = GameObject.Find("Singletons/PlayerData");
        if (playerDataObject != null)
        {
            //PlayerStatusの登録
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
            // PlayerRunTimeStatusの登録
            var playerRunTimeStatus = playerDataObject.GetComponentInChildren<PlayerRunTimeStatus>();
            if (playerRunTimeStatus != null)
            {
                builder.RegisterInstance(playerRunTimeStatus);
                Debug.Log($"PlayerRunTimeStatusが正常に登録されました: {playerRunTimeStatus.name}");
            }
            else
            {
                Debug.LogError("Singletons/PlayerDataの子オブジェクトにPlayerRunTimeStatusコンポーネントが見つかりません");
            }
        }
        else
        {
            Debug.LogError("Singletons/PlayerDataオブジェクトが見つかりません");
        }

        // PlayerPartsを登録
        var playerParts = Object.FindAnyObjectByType<PlayerParts>();
        if (playerParts != null)
        {
            builder.RegisterInstance(playerParts);
            Debug.Log($"PlayerPartsが正常に登録されました: {playerParts.name}");
        }
        else
        {
            Debug.LogError("PlayerオブジェクトにPlayerPartsコンポーネントが見つかりません");
        }

        // PlayerAnimationManagerを登録
        var playerAnimationManager = Object.FindAnyObjectByType<PlayerAnimationManager>();
        if (playerAnimationManager != null)
        {
            builder.RegisterInstance(playerAnimationManager);
            Debug.Log($"PlayerAnimationManagerが正常に登録されました: {playerAnimationManager.name}");
        }
        else
        {
            Debug.LogError("PlayerオブジェクトにPlayerAnimationManagerコンポーネントが見つかりません");
        }

        // PlayerAirCheckerを登録（Playerオブジェクトから取得）
        var playerAirChecker = player.GetComponent<PlayerAirChecker>();
        if (playerAirChecker != null)
        {
            builder.RegisterInstance(playerAirChecker);
            Debug.Log($"PlayerAirCheckerが正常に登録されました: {playerAirChecker.name}");
        }
        else
        {
            Debug.LogError("PlayerオブジェクトにPlayerAirCheckerコンポーネントが見つかりません");
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

        // Controller
        var controller = player.GetComponent<Controller>();
        if (controller != null)
        {
            // 1. インスタンスを登録
            builder.RegisterInstance(controller);
            // 2. 構築後にDIを実行
            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Inject(controller);
            });
            Debug.Log("Controllerに注入予約しました");
        }
        else
        {
            Debug.LogError("PlayerオブジェクトにControllerコンポーネントが見つかりません");
        }
        
        Debug.Log("GameLifetimeScope 登録完了");
    }

}