using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public bool IsRefulHealth = false;
    public bool IsRefulShield = false;
    public float ReplenishAmount = 2f;
    public float BounceHeight = 0.05f;  
    public float RotationSpeed = 30f; 
    private float TargetY = 0;

    void Update()
    {
        Spin();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "LaserSword")
        {
            TriggerPowerUp();
        }
    }

    public void TriggerPowerUp()
    {
        if (IsRefulHealth) 
        {
            Player.Instance.GiveHealth(ReplenishAmount);
            Destroy(gameObject);
        }
        if (IsRefulShield)
        {
            Player.Instance.GiveShield(ReplenishAmount);
            Destroy(gameObject);
        }
    }
    private void Spin()
    {
        float newY = TargetY + Mathf.Sin(Time.time) * BounceHeight;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }
}
