using UnityEngine;

public class Thorn : MonoBehaviour
{
    public float slowMultiplier = 0.4f;
    public float slowDuration = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ApplySlow(slowMultiplier, slowDuration);
        }
    }
}