using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour {
    private void Start() {
        StartCoroutine(Play());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)){
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator Play() {
        yield return new WaitForSeconds(45);
        SceneManager.LoadScene(1);
    }

}
