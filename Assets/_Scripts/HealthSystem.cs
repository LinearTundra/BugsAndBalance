using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour {
    
    [SerializeField] private Slider healthBar;
    private int health = 100;
    private float poisonDamageTime = 0;
    private float poisonTime = 0;
    private float poisonDuration = 0;
    private bool isPoisoned = false;

    private void Update() {
        if (isPoisoned) {
            poisonTime += Time.deltaTime;
            poisonDamageTime += Time.deltaTime;
            if (poisonTime > poisonDuration) isPoisoned = false;
            if (poisonDamageTime > 1) {
                Hit(3);
                poisonDamageTime = 0;
            }
        }
    }

    public void Hit(int damage) {
        health -= damage;
        healthBar.value = health;
        if (health < 1) {
            SceneManager.LoadScene(3);
        }
    }

    public void Poison(int duration) {
        poisonDuration = duration;
        poisonTime = 0;
        isPoisoned = true;
    }

}
