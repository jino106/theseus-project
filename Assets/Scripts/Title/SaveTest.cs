using Unity.VisualScripting;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    [SerializeField] private GameDataManager dataManager;
    [SerializeField] private int slot;

    void Start()
    {
        dataManager.TestSaveGame(slot);
    }
}
