using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroidBoss : Droid
{
    public static DroidBoss Instance { get; private set; }

    public GameObject[] BossPiecesPhase1;
    public GameObject[] BossPiecesPhase2;
    public List<Rigidbody> SlicedBodyParts = new List<Rigidbody>();
    public int Health = 3;
    public GameObject RocketPrefab;
    public GameObject ShootingPos;
    public ParticleSystem DeathVFX;
    private float ChargedAmount;
    public Animator BossAnimator;
    public GameObject Shield;


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        MenuManager.Instance.ShowBossMenu();
    }
    public override void FixedUpdate()
    {
        if(CanShoot)
        {
            ChargeWeapon();
        }
        LockOnToTarget(GameController.Instance.PlayerObject.transform);
    }
    /// <summary>
    /// Charges weapon and checks to see if weapons is ready to fire
    /// </summary>
    public override void ChargeWeapon()
    {
        ChargedAmount += Time.deltaTime;
            
        if (ChargedAmount > WeaponFireTick)
        {
            Shoot(ShootingPos, Player.Instance.transform);
            ChargedAmount = 0;
        }
    }
    /// <summary>
    /// Insantiates Projectile that fires toward the player
    /// </summary>
    public override void Shoot(GameObject shootingPos, Transform player)
    {
        Instantiate(RocketPrefab, shootingPos.transform.position, shootingPos.transform.rotation);
    }
    /// <summary>
    /// records hit and checks if boss is dead
    /// </summary>
    public void TakeDamage()
    {
        Health--;
        if(Health == 0)
        {
            StartPhaseTwo();
        }
    }
    /// <summary>
    /// Moves boss into Sliceable phase
    /// (Swaps animatabale model for Sliceable model)
    /// </summary>
    private void StartPhaseTwo()
    {
        CanShoot = false;
        foreach(GameObject bossPiece in BossPiecesPhase1)
        {
            bossPiece.SetActive(false);
        }
        foreach(GameObject bossPiece in BossPiecesPhase2)
        {
            bossPiece.SetActive(true);
        }
        Shield.SetActive(false);
        DroidController.Instance.AttackAlert.SetActive(true);
        GameController.Instance.SetGameAsSlowMotion();
        GameController.Instance.DestroyAllInstancesOf("Rocket");

        StartCoroutine(StartSlowMotion());
    }
    /// <summary>
    /// Applys explosive force to all sliced pieces
    /// </summary>
    private void ApplyExplosionToBossPieces()
    {   
        DeathVFX.Play();
        foreach(GameObject slicedPartRB in BossPiecesPhase2.Where(rb => rb != null))
        {
            slicedPartRB.GetComponent<Rigidbody>().isKinematic = false;
            slicedPartRB.GetComponent<Rigidbody>().AddExplosionForce(1000, -slicedPartRB.gameObject.transform.up, 1000);
            slicedPartRB.GetComponent<Rigidbody>().useGravity = true;
            slicedPartRB.gameObject.layer = 0;
        }
        foreach(Rigidbody slicedPartRB in SlicedBodyParts.Where(rb => rb != null))
        {
            slicedPartRB.isKinematic = false;
            slicedPartRB.AddExplosionForce(1000, -slicedPartRB.gameObject.transform.up, 1000);
            slicedPartRB.useGravity = true;
            slicedPartRB.gameObject.layer = 0;
        }
    }
    /// <summary>
    /// Turns game into slowmotion for 3 seconds
    /// </summary>
    private IEnumerator StartSlowMotion()
    {
        yield return new WaitForSecondsRealtime(8f);
        GameController.Instance.ResetGameSpeed();
        ApplyExplosionToBossPieces();
        DroidController.Instance.AttackAlert.SetActive(false);
        StartCoroutine(DestroyAfterDelay());
    }
    /// <summary>
    /// Clears Boss after 3 seconds after defeating it
    /// </summary>
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        DroidController.Instance.RecordDroidDeath();
        GameController.Instance.DestroyAllInstancesOf("Boss");
    }
    /// <summary>
    /// Makes boss look at the Player
    /// </summary>
    protected void LockOnToTarget(Transform player)  
    {
        Quaternion _lookRotation = 
            Quaternion.LookRotation((player.position - transform.position).normalized);
        _lookRotation.x = 0;
        _lookRotation.z = 0;
	    transform.rotation = _lookRotation;
    }
}
