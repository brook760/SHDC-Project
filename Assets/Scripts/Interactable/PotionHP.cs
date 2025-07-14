using UnityEngine;

public class PotionHP : MonoBehaviour
{
    public int healthUp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Character>(out var player))
        {
            player.GainHealth(healthUp);
            Destroy(gameObject);
        }
    }
}
