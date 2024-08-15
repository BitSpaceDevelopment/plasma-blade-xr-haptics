using System.Linq;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{

    public bool CanSpawn = false;
    public int SpawnPointsAllocated;
    public float SpawnDelay;
    public float SpawnClock;
    public bool DroidsMoveClockwise = true;
    public GameObject Droid;
    public SphereCollider sphereCollider;

    void Update()
    {
        bool spawnDroid = 
            SpawnPointsAllocated > 0 && 
            CanSpawn && 
            SpawnClock >= SpawnDelay && 
            CheckIfPosIsClear();

        if(spawnDroid)
        {
            SpawnDroid();
        } 
        else
        {
            SpawnClock += Time.deltaTime;
        }
    }
    /// <summary>
    /// Spawns droid and sets up its movement
    /// </summary>
    private void SpawnDroid()
    {
        GameObject droid = Instantiate(Droid, transform.position, transform.rotation);
        if(DroidsMoveClockwise == false)
        {
            droid.GetComponent<DroidMovement_TrainingDroid>().OrbitSpeed = -30;
        }
        SpawnPointsAllocated--;
        SpawnClock = 0;
    }
    /// <summary>
    /// Checks if a droid is covering the spawn space
    /// </summary>
    /// <returns>Retrus a boolean value if location is clear</returns>
    private bool CheckIfPosIsClear()
    {
        Collider[] collidersInside = Physics.OverlapSphere(transform.position, sphereCollider.radius / 2);
        if(collidersInside.Where(collider => collider.gameObject.CompareTag("Droid")).Count() > 0)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Resets this spawnNode
    /// </summary>
    public void ResetNode()
    {
        SpawnPointsAllocated = 0;
        PreventSpawn();
    }
    /// <summary>
    /// prevents the droids from spawning from this node
    /// </summary>
    public void PreventSpawn()
    {
        CanSpawn = false;
        SpawnClock = 0;
    }
}
