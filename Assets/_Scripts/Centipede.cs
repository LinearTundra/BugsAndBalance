using System.Collections;
using UnityEngine;

public class Centipede : MonoBehaviour {
    [SerializeField] private int Speed = 8;
    public GameObject CentepedeClone;
    public GameObject blastZone;
    private Enemy enemy = new Enemy();
    private Vector3 centipede;
    private Vector3 player;
    private Vector3 strafeDir;
    private Vector3 attackDir;
    private int divideCounter = 0;
    private float deltaTime = 0;
    private float strafeTime = 0;
    private float selfDestructTimer = 0;
    private float entangleTimer = 0;
    private float entangleHoldTime = 10;
    private float attackCooldown = 0;
    public bool isEntangling = false;
    public bool isClone = false;

    private void Start() {
        enemy.playerProximity = 3;
        enemy.strafeDistance = 2;
        enemy.dodgeDistance = 2;
        centipede = transform.position;
        strafeDir = enemy.strafeDirection(centipede);
    }

    private void Update() {
        deltaTime = Time.deltaTime;
        attackCooldown += deltaTime;
        strafeTime += deltaTime;
        centipede = transform.position;
        player = GameManager.Player.position;
        
        if (isClone) {
            selfDestruct();
            return;
        }

        if (entangleHoldTime < 1.5f) {
            entangleHoldTime += deltaTime;
            transform.position = player;
            return;
        }

        if (isEntangling) {
            Entangle();
            return;
        }

        if (strafeTime > 1) updateStrafeDir();

        Move();

        if (attackCooldown > Random.Range(2,5)) InitiateAttack();

    }

    private void updateStrafeDir() {
        strafeTime = 0;
        strafeDir = enemy.strafeDirection(centipede);
    }

    private void Move() {
        Vector3 dir = enemy.moveDirection(centipede, player);
        transform.position += (dir*Speed - strafeDir).normalized * Speed * deltaTime;
    }

    private void InitiateAttack() {
        attackDir = enemy.attackDirection(centipede, player);
        attackCooldown = 0;
        if (Random.Range(0,2) == 0 || divideCounter < 1){
            Entangle();
            divideCounter++;
            return;
        }
        Divide();
        divideCounter = 0;
    }

    private void selfDestruct() {
        attackDir = enemy.attackDirection(centipede, player);
        transform.position += attackDir * Speed * deltaTime;
        selfDestructTimer += deltaTime;
        if ((centipede-player).magnitude < 1 || selfDestructTimer > 3) {
            GameObject Blast = Instantiate(blastZone, centipede, Quaternion.identity);
            Blast.layer = LayerMask.NameToLayer("Enemy Projectile");
            Destroy(gameObject);
            Destroy(Blast, .1f);
        }
    }

    private void Entangle() {
        transform.position += attackDir * Speed * 2 * deltaTime;
        entangleTimer += deltaTime;
        if (entangleTimer > 2.5f || (centipede-player).magnitude < 1) {
            if ((centipede-player).magnitude < 1) entangleHoldTime = 0;
            isEntangling = false;
        }
    }

    private void Divide() {
        for (int i=0; i<4; i++) {
            GameObject clone = Instantiate(CentepedeClone, centipede+attackDir, Quaternion.identity);
            Vector3 disperseDir = Random.onUnitSphere;
            disperseDir.z = 0;
            clone.GetComponent<Rigidbody2D>().AddForce(disperseDir*Speed, ForceMode2D.Impulse);
            clone.GetComponent<Centipede>().isClone = true;
        }
    }

}
