using System.Collections;
using UnityEngine;

public class Beetle : MonoBehaviour {
    
    [SerializeField] private int Speed = 8;
    [SerializeField] private int knockbackForce = 10;
    [SerializeField] private float knockbackTime = .1f;
    public GameObject Pincher;
    private Rigidbody2D rb;
    private Enemy enemy = new Enemy();
    private Vector3 beetle;
    private Vector3 player;
    private Vector3 strafeDir;
    private Vector3 attackDir;
    private float strafeTime = 0;
    private float attackCooldown = 0;
    private float attackTime = 0;
    private bool isTackling = false;
    private bool isPinching = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        enemy.playerProximity = 2;
        enemy.strafeDistance = 2;
        enemy.dodgeDistance = 2;
        beetle = transform.position;
        strafeDir = enemy.strafeDirection(beetle);
    }

    private void Update() {
        attackCooldown += Time.deltaTime;
        strafeTime += Time.deltaTime;
        beetle = transform.position;
        player = GameManager.Player.position;
        
        if (isTackling) {
            Tackle();
            return;
        }

        if (isPinching) {
            Pinch();
            return;
        } 

        if (strafeTime > 1) updateStrafeDir();

        if (attackCooldown > Random.Range(2,5)) InitiateAttack();

        Pincher.transform.SetLocalPositionAndRotation(enemy.attackDirection(beetle, player), Quaternion.identity);
        Move();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (isTackling) {
            isTackling = false;
            StopCoroutine(knockback(other));
            StartCoroutine(knockback(other));
        }
        attackCooldown = 0;
    }

    private void updateStrafeDir() {
        strafeTime = 0;
        strafeDir = new Vector3 (0, 0, 0);
        if ((player-beetle).magnitude<6)
            strafeDir = enemy.strafeDirection(beetle);
    }

    private void Move() {
        Vector3 dir = enemy.moveDirection(beetle, player);
        rb.linearVelocity += (Vector2)(dir*Speed - strafeDir).normalized * Speed * Time.deltaTime;
    }

    private void InitiateAttack() {
        attackDir = enemy.attackDirection(beetle, player);
        attackTime = 0;
        if (Random.Range(0,2) == 0){
            isTackling = true;
            Tackle();
            return;
        }
        Pincher.GetComponent<BeetlePinchAttack>().isPinching = true;
        isPinching = true;
        Pinch();
    }

    private void Tackle() {
        rb.linearVelocity += (Vector2)attackDir * Speed * 2 * Time.deltaTime;
        attackTime += Time.deltaTime;
        if (attackTime < .5f) return;
        isTackling = false;
        attackCooldown = 0;
    }

    private void Pinch() {
        rb.linearVelocity += (Vector2)attackDir * Speed * Time.deltaTime;
        attackTime += Time.deltaTime;
        if (attackTime < 1 && Pincher.GetComponent<BeetlePinchAttack>().isPinching == true) return;
        isPinching = false;
        attackCooldown = 0;
        Pincher.GetComponent<BeetlePinchAttack>().isPinching = false;
    }

    private IEnumerator knockback(Collision2D other) {
        other.rigidbody.AddForce(attackDir*knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackTime);
        other.rigidbody.linearVelocity = Vector3.zero;
    }

}
