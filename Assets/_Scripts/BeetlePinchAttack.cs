using UnityEngine;

public class BeetlePinchAttack : MonoBehaviour {

    public bool isPinching = false;
    private int pinchCount = 0;

    private void OnTriggerStay2D(Collider2D collision) {
        if (isPinching) {
            isPinching = false;
            Debug.Log("Pinched");
            Debug.Log(++pinchCount);
        }
    }
    
}
