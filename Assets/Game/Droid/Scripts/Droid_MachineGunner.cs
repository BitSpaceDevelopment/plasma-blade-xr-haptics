using UnityEngine;
using Greyman;

public class Droid_MachineGunner : Droid
{
    private Transform Player;
    public GameObject[] ShootingPositions;
    private float ChargedAmountOne;
    private float ChargedAmountTwo;
    private bool FirstCharge = true;
    public ShootVFXHandler[] ShootVFXHandlers;
    public float InitialCharge = 3;
    private float InitialChargeTime;

    // Start is called before the first frame update
    void Start()
    {
        DroidType = "Machine Gunner";
        Player = GameController.Instance.PlayerObject.transform;
        OffScreenIndicator.Instance.AddDroid(gameObject);

        //Offsets shooting time
        ChargedAmountTwo = -WeaponFireTick / 2;
    } 
    /// <summary>
    /// Charges weapon and checks to see if weapons is ready to fire
    /// Allows for two guns
    /// </summary>
    public override void ChargeWeapon()
    {
        if(InitialChargeTime > InitialCharge)
        {
            FixTimeOffset();

            if (ChargedAmountTwo == 0)
            {
                PlayWarmupFX(ShootVFXHandlers[1]);
            }
            else if (ChargedAmountOne == 0)
            {
                PlayWarmupFX(ShootVFXHandlers[0]);
            } 

            ChargedAmountOne += Time.deltaTime;
            ChargedAmountTwo += Time.deltaTime;

            if (ChargedAmountOne > WeaponFireTick)
            {
                Shoot(ShootingPositions[0], Player);
                ChargedAmountOne = 0;
            } 
            else if (ChargedAmountTwo > WeaponFireTick)
            {
                Shoot(ShootingPositions[1], Player);
                ChargedAmountTwo = 0;
            }
        }
        else
        {
            InitialChargeTime += Time.deltaTime;
        }
    }
    /// <summary>
    /// Sets a time offset of two guns
    /// </summary>
    void FixTimeOffset()
    {
        if(FirstCharge && ChargedAmountTwo > 0)
        {
            ChargedAmountTwo = 0;
            FirstCharge = false;
        }
    }
}
