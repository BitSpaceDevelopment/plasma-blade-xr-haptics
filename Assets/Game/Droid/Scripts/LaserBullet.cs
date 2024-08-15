using System.Collections;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    [Header("Laser Settings")]
    public float Lifespan = 10f;
    public float Damage;
    private bool HasReflected = false;
    public Vector3 Direction;
    public Rigidbody LaserRigidbody;
    public Vector3 StartingPoint;

    [Header("Prefabs")]
    public GameObject LaserHitPrefab;

    public void Start()
    {   
        StartCoroutine(DestroyAfterDelay());
    }

    private void OnTriggerEnter(Collider other)
    {  
        if (other.tag == "LaserSword")
        {
            OnSaberHit();
        }
        else if (other.tag == "Droid")
        {
            OnEntityHit(other);
        }
        else if (other.tag == "Player")
        {
            OnPlayerHit(other);
        }
    }
    /// <summary>
    /// Has reflectable projects reflect
    /// </summary>
    private void OnSaberHit()
    {
        if(HasReflected == false)
        {
            ReflectProjectile();
        }
    }
    /// <summary>
    /// Damages enitity if the projectile has been reflected
    /// </summary>
    private void OnEntityHit(Collider other)
    {
        if(HasReflected){
            other.gameObject.GetComponentInChildren<DroidHealth>().ApplyDamage();
            LaserFX(other);
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Damages player on projectile hit
    /// </summary>
    private void OnPlayerHit(Collider other)
    {
        other.gameObject.GetComponent<Player>().ApplyDamage(Damage);
        Destroy(gameObject);
    }
    /// <summary>
    /// Handles hit VFX
    /// </summary>
    private void LaserFX(Collider other)
    {
        GameObject laserHit = Instantiate(LaserHitPrefab, transform.position, transform.rotation);
        LaserHit laserHitScript = laserHit.GetComponent<LaserHit>();
        laserHitScript.SetHit(transform, other.transform, false);
    }
    /// <summary>
    /// Sends project back at the position it was shot from
    /// </summary>
    private void ReflectProjectile()
    {
        LaserRigidbody.velocity = -LaserRigidbody.velocity;
        transform.rotation = Quaternion.LookRotation((StartingPoint - transform.position).normalized);
        HasReflected = true;  
    }
    /// <summary>
    /// deleted projectile after time
    /// </summary>
    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(Lifespan);
        Destroy(gameObject);
    }
}