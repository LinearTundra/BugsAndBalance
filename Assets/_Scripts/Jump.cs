using UnityEngine;
using UnityEngine.SceneManagement;

public class Jump : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        SceneManager.LoadScene(2);
    }
}
