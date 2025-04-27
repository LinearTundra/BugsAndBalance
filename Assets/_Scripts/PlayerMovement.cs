using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private int Speed = 10;
    public ClickMove cb;
    private Rigidbody2D rb;
    
    private void Start() {
        cb = GetComponent<ClickMove>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Move();
    }

    private void Move() {
        Vector3 dir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) dir.y += 1;
        if (Input.GetKey(KeyCode.S)) dir.y -= 1;
        if (Input.GetKey(KeyCode.D)) dir.x += 1;
        if (Input.GetKey(KeyCode.A)) dir.x -= 1;
        transform.position += dir.normalized * Speed * Time.deltaTime;
    }
}
