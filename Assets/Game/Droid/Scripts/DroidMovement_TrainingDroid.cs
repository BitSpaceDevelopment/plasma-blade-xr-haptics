using UnityEngine;

public class DroidMovement_TrainingDroid : MonoBehaviour
{
    [Header("Orbit")]
    public float OrbitSpeed;
    
    private Transform playerHeadset;
    private Transform CenterOfGame;

    // Start is called before the first frame update
    void Start()
    {
        playerHeadset = GameController.Instance.PlayerObject.transform;
        CenterOfGame = GameController.Instance.Map.transform;
    }

    // Update is called once per frame
    void Update()
    {
        LockOnToTarget(playerHeadset);
        MovementBehaviour();
    }
    /// <summary>
    /// Makes droid look at player
    /// </summary>
    protected void LockOnToTarget(Transform player) 
    {
        Quaternion _lookRotation = 
            Quaternion.LookRotation((player.position - transform.position).normalized);

	    transform.rotation = _lookRotation;
    }
    /// <summary>
    /// Handles the way the droid moves
    /// </summary>
    protected void MovementBehaviour()
    {
        transform.RotateAround (CenterOfGame.position, Vector3.up, OrbitSpeed * Time.deltaTime);
    }
}
