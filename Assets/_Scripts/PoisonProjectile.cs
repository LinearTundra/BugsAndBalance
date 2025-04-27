using UnityEngine;

public class PoisonProjectile : MonoBehaviour {

    public HealthSystem player;

    private void OnTriggerEnter2D(Collider2D other) {
        player = other.gameObject.GetComponent<HealthSystem>();
        player.Poison(5);
        player = null;
    }

}
