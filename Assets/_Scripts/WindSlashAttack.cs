using UnityEngine;

public class WindSlashAttack : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Wind Slashed");
    }

}
