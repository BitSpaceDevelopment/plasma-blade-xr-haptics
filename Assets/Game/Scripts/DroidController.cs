using UnityEngine;

public class DroidController : MonoBehaviour
{
    public static DroidController Instance { get; private set; }
    public GameObject[] SpawnNodesForTDroids;
    public GameObject[] SpawnNodesForMGDroids;
    public GameObject SpawnNodeForBossDroid;
    public int PointsToAllocateForTDroids = 0;
    public int PointsToAllocateForMGDroids = 0;
    public int DroidsAlive = 0;
    public int DroidsKilledTotal = 0;
    public GameObject AttackAlert;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartRound(int amountForTDroids, int amountForMGDroids)
    {
        PointsToAllocateForTDroids = amountForTDroids;
        PointsToAllocateForMGDroids = amountForMGDroids;

        DroidsAlive = amountForTDroids + amountForMGDroids;
        AllocatePoints();
        StartSpawning();
    }
    public void StartRound()
    {
        DroidsAlive = 1;
        SpawnBoss();

    }
    public void SpawnBoss()
    {
        SpawnHandler spawnScript = SpawnNodeForBossDroid.GetComponent<SpawnHandler>();
        spawnScript.SpawnPointsAllocated = 1;
    }
    public void AllocatePoints()
    {
        for(int i = 0; i < PointsToAllocateForTDroids; i++)
        {
            GameObject spawnNode = GetRandomSpawnNode(SpawnNodesForTDroids);
            SpawnHandler spawnScript = spawnNode.GetComponent<SpawnHandler>();
            spawnScript.PreventSpawn();
            spawnScript.SpawnPointsAllocated += 1;
            spawnScript.SpawnDelay = GetRandomDelay(i);
        }
        for(int i = 0; i < PointsToAllocateForMGDroids; i++)
        {
            GameObject spawnNode = GetRandomSpawnNode(SpawnNodesForMGDroids);
            SpawnHandler spawnScript = spawnNode.GetComponent<SpawnHandler>();
            spawnScript.PreventSpawn();
            spawnScript.SpawnPointsAllocated += 1;
            spawnScript.SpawnDelay = GetRandomDelay(i);
        }
    }
    public void StartSpawning()
    {
        foreach(GameObject SpawnNode in SpawnNodesForTDroids)
        {
            SpawnNode.GetComponent<SpawnHandler>().CanSpawn = true;
        }
        foreach(GameObject SpawnNode in SpawnNodesForMGDroids)
        {
            SpawnNode.GetComponent<SpawnHandler>().CanSpawn = true;
        }
        PointsToAllocateForMGDroids = 0;  
    }
    public void StopSpawning()
    {
        foreach(GameObject SpawnNode in SpawnNodesForTDroids)
        {
            SpawnNode.GetComponent<SpawnHandler>().CanSpawn = false;
        }
        foreach(GameObject SpawnNode in SpawnNodesForMGDroids)
        {
            SpawnNode.GetComponent<SpawnHandler>().CanSpawn = false;
        }
    }
    public GameObject GetRandomSpawnNode(GameObject[] nodesToPickFrom)
    {
        int rndNum = Random.Range(0, nodesToPickFrom.Length - 1);
        return nodesToPickFrom[rndNum];
    }
    public float GetRandomDelay(int baseDelay)
    {
        float rndNum = Random.Range(baseDelay, baseDelay + 5);
        return rndNum;
    } 
    public void RecordDroidDeath()
    {
        DroidsAlive--;
        DroidsKilledTotal++;
        CheckIfLevelIsOver();
    }
    public void CheckIfLevelIsOver()
    {
        if(DroidsAlive == 0)
        {
            StopSpawning();
            LevelController.Instance.EndLevel();
        }
    }
    public void ResetDroidController()
    {
        DroidsAlive = 0;
        DroidsKilledTotal = 0;
        PointsToAllocateForTDroids = 0;
    }
    public void ResetAllNodes()
    {
        foreach(GameObject spawnNode in SpawnNodesForTDroids)
        {
            spawnNode.GetComponent<SpawnHandler>().ResetNode();
        }
        foreach(GameObject spawnNode in SpawnNodesForMGDroids)
        {
            spawnNode.GetComponent<SpawnHandler>().ResetNode();
        }
    }
}
