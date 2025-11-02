using UnityEngine;

public class PressMachineSound : MonoBehaviour
{
    public bool isPlayer = false;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayer = false;
        }
    }
}
