using UnityEngine;

public abstract class Droid : MonoBehaviour
{
    public GameObject LaserPrefab;
    public float WeaponFireTick = 2.5f;
    public bool CanShoot;
    public string DroidType;


    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        if(CanShoot)
        {
            ChargeWeapon();
        }
    }
    /// <summary>
    /// Charges weapon and checks to see if weapons is ready to fire
    /// </summary>
    public virtual void ChargeWeapon(){}
    /// <summary>
    /// Plays warmup VFX
    /// </summary>
    public void PlayWarmupFX(ShootVFXHandler VFXHandler)
    {
        VFXHandler.StartWarmUp();
    }
    /// <summary>
    /// Insantiates a projectile that fires toward the player
    /// </summary>
    public virtual void Shoot(GameObject shootingPos, Transform player)
    {
        GameObject projectile = Instantiate(LaserPrefab, shootingPos.transform.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.LookRotation((player.position - transform.position).normalized);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(projectile.transform.forward * 5, ForceMode.Impulse);
        projectile.GetComponent<LaserBullet>().Direction = player.transform.position;
        projectile.GetComponent<LaserBullet>().StartingPoint = transform.position;
    }
}
