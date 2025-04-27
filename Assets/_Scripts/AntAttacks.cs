using UnityEngine;

public class AntAttacks : MonoBehaviour {
    public bool isKicking = false;
    public bool isPunching = false;

    private void OnTriggerStay2D(Collider2D other) {
        if (isKicking) {
            isKicking = false;
            other.gameObject.GetComponent<HealthSystem>().Hit(7);
        }

        else if (isPunching) {
            isPunching = false;
            other.gameObject.GetComponent<HealthSystem>().Hit(5);
        }
    }
}
