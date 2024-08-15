using UnityEngine;

public class DroidMovment_MachineGunner : MonoBehaviour
{
    private Transform playerHeadset;

    // Start is called before the first frame update
    void Start()
    {
        playerHeadset = GameController.Instance.PlayerObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        LockOnToTarget(playerHeadset);
    }
    /// <summary>
    /// Has droid look at player
    /// </summary>
    protected void LockOnToTarget(Transform player) 
    {
        Quaternion _lookRotation = 
            Quaternion.LookRotation((player.position - transform.position).normalized);

	    transform.rotation = _lookRotation;
    }
}
