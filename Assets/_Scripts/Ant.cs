using System.Collections;
using UnityEngine;

public class Ant : MonoBehaviour {
    
    [SerializeField] private int Speed = 8;
    public GameObject AntAttacker;
    public GameObject PoisonBall;
    private Rigidbody2D rb;
    private Enemy enemy = new Enemy();
    private Vector3 ant;
    private Vector3 player;
    private Vector3 strafeDir;
    private Vector3 attackDir;
    private float strafeTime = 0;
    private float attackCooldown = 0;
    private float attackTime = 0;
    private bool isKicking = false;
    private bool isPunching = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        enemy.playerProximity = 1;
        enemy.strafeDistance = 2;
        enemy.dodgeDistance = 2;
        ant = transform.position;
        strafeDir = enemy.strafeDirection(ant);
    }

    private void Update() {
        attackCooldown += Time.deltaTime;
        strafeTime += Time.deltaTime;
        ant = transform.position;
        player = GameManager.Player.position;
        
        Move();

        if (isKicking) {
            Kick();
            return;
        }

        if (isPunching) {
            Punch();
            return;
        } 

        if (strafeTime > 1) updateStrafeDir();

        if (attackCooldown > Random.Range(2,5)) InitiateAttack();
    }

    private void updateStrafeDir() {
        strafeTime = 0;
        strafeDir = new Vector3 (0, 0, 0);
        if ((player-ant).magnitude<enemy.playerProximity*3)
            strafeDir = enemy.strafeDirection(ant);
    }

    private void Move() {
        Vector3 dir = enemy.moveDirection(ant, player);
        rb.linearVelocity += (Vector2)(dir*Speed + strafeDir).normalized * Speed * Time.deltaTime;
    }

    private void InitiateAttack() {
        strafeDir = Vector3.zero;
        attackTime = 0;
        attackDir = enemy.attackDirection(ant, player);
        AntAttacker.transform.localPosition = Vector3.zero;
        Poison();
        return;
        /*
        int attackType = Random.Range(0, 3);
        if (attackType == 0){
            AntAttacker.GetComponent<AntAttacks>().isKicking = true;
            isKicking = true;
            Kick();
        }
        else if (attackType == 0){
            AntAttacker.GetComponent<AntAttacks>().isPunching = true;
            isPunching = true;
            Punch();
        }
        else {
            Poison();
        }
        */
    }

    private void Kick() {
        AntAttacker.transform.position += attackDir * Speed * 2 * Time.deltaTime;
        attackTime += Time.deltaTime;
        if (attackTime < .1f && AntAttacker.GetComponent<AntAttacks>().isKicking == true) return;
        AntAttacker.GetComponent<AntAttacks>().isKicking = false;
        isKicking = false;
        attackCooldown = 0;
    }

    private void Punch() {
        AntAttacker.transform.position += attackDir * Speed * 2 * Time.deltaTime;
        attackTime += Time.deltaTime;
        if (attackTime < .1f && AntAttacker.GetComponent<AntAttacks>().isPunching == true) return;
        AntAttacker.GetComponent<AntAttacks>().isPunching = false;
        isPunching = false;
        attackCooldown = 0;
    }

    private void Poison() {
        GameObject poison = Instantiate(PoisonBall, ant+attackDir, Quaternion.identity);
        poison.GetComponent<Rigidbody2D>().AddForce(attackDir*Speed*3, ForceMode2D.Impulse);
        poison.layer = LayerMask.NameToLayer("Enemy Projectile");
        attackCooldown = 0;
        Destroy(poison, 1);
    }

}
