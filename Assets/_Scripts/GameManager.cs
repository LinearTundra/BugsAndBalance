using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public static Transform Player;

    private void Start() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

}
