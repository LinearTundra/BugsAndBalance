using UnityEngine;

public class SwordAttack : MonoBehaviour {

    [SerializeField] private int damage = 10;
    public Collider2D swordCollider;
    private Quaternion originalRotation;
    private GameObject currentEnemy = null;
    private Transform Player = null;
    private bool isSwinging = false;
    private float swingTime = 0;

    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        currentEnemy = GameObject.FindGameObjectWithTag("Enemy");
        swordCollider = GetComponent<Collider2D>();
        originalRotation = transform.rotation;
        swordCollider.enabled = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SwingSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        HealthSystem health = other.gameObject.GetComponent<HealthSystem>();
        health.Hit(damage);
    }

    private void SwingSword() {
        if (!isSwinging) {
            swordCollider.enabled = true;
            isSwinging = true;
            return;
        }
        else if (swingTime > .05f) {
            swordCollider.enabled = false;
            isSwinging = false;
            swingTime = 0;
        }
        else if (isSwinging) swingTime += Time.deltaTime;
    }

    private void setPosition() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos -= Player.position;
        mousePos.z = -0.5f;
        mousePos = mousePos.normalized * 2 + Player.position;
        transform.position = mousePos;
    }

    private void setRotation() {
        Vector3 look = transform.InverseTransformPoint(Player.position);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg + 45;
        if (isSwinging) angle += 45;
        transform.Rotate(0, 0, angle);
    }

}
