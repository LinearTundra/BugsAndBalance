using UnityEngine;

public class Enemy {

    public int playerProximity;
    public int strafeDistance;
    public int dodgeDistance;
    
    public Vector3 moveDirection(Vector3 current, Vector3 player) {
        float distance = (current-player).magnitude;
        Vector3 dir =  Vector3.zero;
        if (distance > playerProximity+2) dir = player-current;
        else if (distance < playerProximity) dir = current-player;
        return dir.normalized;
    }

    public Vector3 attackDirection(Vector3 current, Vector3 player) {
        return (player-current).normalized;
    }

    public Vector3 strafeDirection(Vector3 current) {
        Vector2 dir = Random.insideUnitCircle;
        current.x += dir.x * strafeDistance;
        current.y += dir.y * strafeDistance;
        return current;
    }

    public Vector3 dodgeDirection(Vector3 current) {
        Vector3 dir = Random.onUnitSphere;
        current.x += dir.x * dodgeDistance;
        current.y += dir.y * dodgeDistance;
        return current;
    }
    
}
