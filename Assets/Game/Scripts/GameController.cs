using UnityEngine;
using Greyman;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("GameObjects")]
    public float GameTime;
    public GameObject PlayerObject;
    public GameObject Map;
    public bool FocusDebug;
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

    public void FixedUpdate()
    {
        GameTime += Time.deltaTime;
    }
    public void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus)
        {
            ResetGameSpeed();
        }
        else
        {
            PauseGame();
        }            
    }
    public void StartGame()
    {
        LevelController.Instance.NextLevel();
        Time.timeScale = 1F;
    }
    public void DestroyAllInstancesOf(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach(GameObject gameObject in gameObjects)
        {
            Destroy(gameObject);
        }
    }
    public void SetGameAsSlowMotion()
    {
        Time.timeScale = 0.1F;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
    public void PauseGame()
    {
        Time.timeScale = 0.001f;
        Time.fixedDeltaTime = 0.0001f;
    }
    public void ResetGameSpeed()
    {
        Time.timeScale = 1F;
        Time.fixedDeltaTime = 0.02F;
    }
    public void TriggerGameOver()
    {
        DroidController.Instance.ResetAllNodes();
        DestroyAllInstancesOf("Droid");
        OffScreenIndicator.Instance.ResetIndicators();
        DestroyAllInstancesOf("Laser");
        DestroyAllInstancesOf("PowerOrb");
        DestroyAllInstancesOf("Rocket");
        DestroyAllInstancesOf("Boss");


        MenuManager.Instance.GameoverScreen();
    }
    public void ResetGame()
    {
        Player.Instance.ResetPlayer();
        LevelController.Instance.ResetLevelController();
        DroidController.Instance.ResetDroidController();
        
        GameTime = 0;
    }
}
