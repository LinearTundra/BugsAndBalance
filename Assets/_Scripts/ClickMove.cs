using UnityEngine;

public class ClickMove : MonoBehaviour {

    [SerializeField] private int Speed = 10;
    public GameObject Sword;
    private GameObject[] Enemies;
    private GameObject currentEnemy = null;
    private Rigidbody2D rb;
    private Enemy enemy = new Enemy();
    private Vector3 player;
    private Vector3 strafeDir = Vector3.zero;
    private Vector2 mouseWorld;
    private Vector2 path;
    private float strafeTime = 0;
    private float enemyProximity = 0.5f;
    private bool isEnemy = false;

    private void Start() {
        mouseWorld = transform.position;
        rb = GetComponent<Rigidbody2D>();
        enemy.playerProximity = 0;
        enemy.strafeDistance = 1;
        enemy.dodgeDistance = 2;
    }

    private void Update() {
        player = transform.position;
        positionSword();
        Move();
    }

    private void Move() {
        if (Input.GetMouseButton(0)){
            mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            checkEnemy();
            mouseWorld.y += .9f;
        }
        path = (mouseWorld - (Vector2)player).normalized;
        if (isEnemy) {
            path = enemy.moveDirection(player, currentEnemy.transform.position);
            rb.linearVelocity += path * Speed * Time.deltaTime;
            Strafe();
            return;
        }
        if (((Vector2)player-mouseWorld).magnitude < enemyProximity) {
            rb.linearVelocity = Vector2.zero;
            mouseWorld = (Vector2)player;
            return;
        }
        rb.linearVelocity += path * Speed * Time.deltaTime;
    }

    private void Strafe() {
        if (currentEnemy != null) {
            mouseWorld = (Vector2)currentEnemy.transform.position;
        }
        strafeTime += Time.deltaTime;
        if (strafeTime < 1) return;
        strafeDir = enemy.strafeDirection(player);
        strafeTime = 0;
        rb.linearVelocity += (Vector2)strafeDir * Time.deltaTime * Speed;
    }

    private void checkEnemy() {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Enemies == null || Enemies.Length == 0) return;
        Vector2 tempMouse = mouseWorld;
        tempMouse.y += .45f;
        foreach (GameObject enemy in Enemies) {
            float variance = Vector2.Distance(tempMouse, enemy.transform.position);
            if (variance < 1) {
                currentEnemy = enemy;
                enemyProximity = 3;
                isEnemy = true;
                return;
            }
        }
        enemyProximity = .5f;
        currentEnemy = null;
        isEnemy = false;
    }

    private void positionSword() {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition)-player;
        if (currentEnemy != null)
            dir = enemy.attackDirection(player, currentEnemy.transform.position);
        Sword.transform.SetLocalPositionAndRotation(dir*2, Quaternion.identity);
    }
//temp
}
