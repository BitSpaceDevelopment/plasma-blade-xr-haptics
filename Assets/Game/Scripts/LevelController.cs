using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    [Header("Level Info")]
    public int MaxAliveTrainingDroidCount = 0;
    public int MaxAliveMachineGunCount = 0;

    public int CountdownNumber = 9;
    public GameObject CountdownObject;
    public GameObject LevelText;
    public int Level = 0;

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
    public void EndLevel()
    {
        StartCoroutine(StartCountdown());
    }
    public void NextLevel()
    {
        Level++;
        UpdateLevelText();
        if(Level % 10 != 0)
        {
            MaxAliveTrainingDroidCount = (int)(Math.Log(Level, 10) * 4 + 1);
            MaxAliveMachineGunCount = (int)(Math.Log(Level - 4, 10) * 3);
            MaxAliveMachineGunCount = MaxAliveMachineGunCount < 0 ? 0 : MaxAliveMachineGunCount;

            DroidController.Instance.StartRound(MaxAliveTrainingDroidCount, MaxAliveMachineGunCount);
        }
        else
        {
            DroidController.Instance.StartRound();
        }
    }
    public void ResetLevelController()
    {
        Level = 0;
        MaxAliveTrainingDroidCount = 0;
    }

    public void ChangeText(int number)
    {
        CountdownObject.GetComponent<TextMeshProUGUI>().text = number.ToString();
    }
    
    private IEnumerator StartCountdown()
    {
        CountdownObject.SetActive(true);
        int timer = CountdownNumber;
        while (timer >= 0)
        {
            yield return new WaitForSeconds(1f);
            if(timer <= 5)
            {
                ChangeText(timer);                
            }
            timer -= 1;
        }
        if (timer < 0)
        {
            NextLevel();
            CountdownObject.SetActive(false);
            CountdownObject.GetComponent<TextMeshProUGUI>().text = "5";
        }
    }
    private void UpdateLevelText()
    {
        LevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + Level;
    }
}
