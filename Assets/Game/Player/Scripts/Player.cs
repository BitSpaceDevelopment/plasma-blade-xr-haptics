using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public GameObject viewedWallPosition;
    public float MaxHealth = 12;
    public float Health = 12;
    public float MaxShield = 3;
    public float Shield = 0;
    public GameObject GrabbedRocket;

    public Image healthBar;
    public Image shieldBar;
    public Sprite[] healthSprites;
    public Sprite[] shieldSprites;
    public Camera PlayerCam;
    public GameObject Canvas;


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

    RaycastHit hit;

    public void Update()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if(hit.transform.tag == "Wall")
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                viewedWallPosition = hit.transform.gameObject;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);
        }
    }
    public void ApplyDamage(float damage)
    {
        if (Shield > 0)
        {
            if (Shield >= damage)
            {
                Shield -= damage;
                damage = 0;
            }
            else
            {
                damage -= Shield;
                Shield = 0;
            }
        }

        if (damage > 0)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
            }
        }

        UpdateHealth();
        UpdateShield();
    }
    public void GiveHealth(float healthToGive)
    {
        Health += healthToGive;
        if(Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        UpdateHealth();
    }
    public void GiveShield(float shieldToGive)
    {
        Shield += shieldToGive;
        if(Shield > MaxShield)
        {
            Shield = MaxShield;
        }
        UpdateShield();
    }
    public void CheckIfPlayerIsDead()
    {
        if(Health <= 0)
        {
            GameController.Instance.TriggerGameOver();
        }
    }
    public void ResetPlayer()
    {
        Health = 12;
        UpdateHealth();
        Shield = 0;
        UpdateShield();
    }
    public void UpdateHealth()
    {
        healthBar.sprite = healthSprites[(int)Health];
        CheckIfPlayerIsDead();
    }
    public void UpdateShield()
    {
        int shieldIndex = Mathf.CeilToInt((Shield / MaxShield) * (shieldSprites.Length - 1));
        shieldBar.sprite = shieldSprites[shieldIndex];
    }
}
