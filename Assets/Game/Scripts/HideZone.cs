using UnityEngine;

public class HideZone : MonoBehaviour
{
    private PlayerController player;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (player == null)
        {
            player = other.GetComponent<PlayerController>();
        }

        if (player == null) return;

        if (!player.IsSneaking)
        {
            Debug.Log("FAIL: Not sneaking!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController p = other.GetComponent<PlayerController>();

        if (p != null && p == player)
        {
            player = null;
        }
    }
}