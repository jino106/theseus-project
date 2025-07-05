using UnityEngine;

/// <summary>
/// 錆びたレバーを操作するためのスクリプト
/// </summary>
public class RustyLever : MonoBehaviour
{
    [SerializeField]
    private GameObject pivot; // RotationPivotオブジェクトをアサイン
    [SerializeField]
    private GameObject player; // Playerオブジェクトをアサイン

    private bool isLeverRotated = false; // レバーが傾いているかどうか

    private Quaternion initialRotation; // 初期Rotationを保存

    void Start()
    {
        if (pivot == null)
        {
            Debug.LogError("Leverオブジェクトがアサインされていません！");
        }
        if (player == null)
        {
            Debug.LogError("Playerオブジェクトがアサインされていません！");
        }

        // 初期Rotationを保存
        initialRotation = pivot.transform.localRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != null && collision.gameObject == player)
        {
            
        }
    }

    public void RotateLever()
    {
        if (pivot != null)
        {
            if (!isLeverRotated)
            {
                // レバーを傾ける処理
                pivot.transform.Rotate(Vector3.forward, 45f);
                isLeverRotated = true;
            }
            else
            {
                // レバーを元の状態に戻す処理
                ResetLeverRotation();
            }
        }
        else
        {
            Debug.LogError("Leverオブジェクトがアサインされていません！");
        }
    }

    private void ResetLeverRotation()
    {
        // レバーを初期状態に戻す
        pivot.transform.localRotation = Quaternion.identity;
        isLeverRotated = false;
    }
}
