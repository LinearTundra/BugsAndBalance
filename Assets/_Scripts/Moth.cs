using UnityEngine;

public class Moth : MonoBehaviour {
    
    [SerializeField] private int Speed = 8;
    public GameObject WindSlashProjectile;
    public GameObject MothClone;
    private Rigidbody2D rb;
    private Enemy enemy = new Enemy();
    private Vector3 moth;
    private Vector3 player;
    private Vector3 strafeDir;
    private Vector3 attackDir;
    private int mirageCounter = 0;
    private float strafeTime = 0;
    private float attackCooldown = 0;
    public bool isClone = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        enemy.playerProximity = 4;
        enemy.strafeDistance = 2;
        enemy.dodgeDistance = 2;
        moth = transform.position;
        strafeDir = enemy.strafeDirection(moth);
    }

    private void Update() {
        attackCooldown += Time.deltaTime;
        strafeTime += Time.deltaTime;
        moth = transform.position;
        player = GameManager.Player.position;
        
        if (strafeTime > 1) updateStrafeDir();

        Move();

        if (isClone) return;

        if (attackCooldown > Random.Range(2,5)) InitiateAttack();

    }

    private void updateStrafeDir() {
        strafeTime = 0;
        strafeDir = enemy.strafeDirection(moth);
    }

    private void Move() {
        Vector3 dir = enemy.moveDirection(moth, player);
        rb.linearVelocity += (Vector2)(dir*Speed - strafeDir).normalized * Speed * Time.deltaTime;
    }

    private void InitiateAttack() {
        attackDir = enemy.attackDirection(moth, player);
        attackCooldown = 0;
        if (Random.Range(0,2) == 0 || mirageCounter < 2){
            mirageCounter++;
            WindSlash();
            return;
        }
        Mirage();
        mirageCounter = 0;
    }

    private void WindSlash() {
        GameObject windslash = Instantiate(WindSlashProjectile, moth+attackDir, Quaternion.identity);
        windslash.GetComponent<Rigidbody2D>().AddForce(attackDir*Speed*3, ForceMode2D.Impulse);
        windslash.layer = LayerMask.NameToLayer("Enemy Projectile");
        Destroy(windslash, 1);
    }

    private void Mirage() {
        for (int i=0; i<4; i++) {
            GameObject clone = Instantiate(MothClone, moth+attackDir, Quaternion.identity);
            Vector3 disperseDir = Random.onUnitSphere;
            disperseDir.z = 0;
            clone.GetComponent<Rigidbody2D>().AddForce(disperseDir*Speed, ForceMode2D.Impulse);
            clone.GetComponent<Moth>().isClone = true;
            Destroy(clone, 5);
        }
    }

}
