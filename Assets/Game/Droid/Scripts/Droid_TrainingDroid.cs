using UnityEngine;
using Greyman;

public class Droid_TrainingDroid : Droid
{
    [Header("Weapon Settings")]
    private Transform Player;
    public GameObject ShootingPosition;
    public AudioSource ShootingSound;
    public AudioSource WarmupSound;
    private float ChargedAmount;
    public ShootVFXHandler ShootVFX;

    // Start is called before the first frame update
    void Start()
    {
        DroidType = "Training Droid";

        Player = GameController.Instance.PlayerObject.transform;
        OffScreenIndicator.Instance.AddDroid(gameObject);
    }
    /// <summary>
    /// Charges weapon and checks to see if weapons is ready to fire
    /// </summary>
    public override void ChargeWeapon()
    {
        if (ChargedAmount == 0)
        {
            PlayWarmupFX(ShootVFX);
        }
        ChargedAmount += Time.deltaTime;
            
        if (ChargedAmount > WeaponFireTick)
        {
            Shoot(ShootingPosition, Player);
            ChargedAmount = 0;
        }
    }
}
