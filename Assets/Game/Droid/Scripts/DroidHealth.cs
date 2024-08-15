using UnityEngine;
using Greyman;
using UnityEngine.InputSystem;


public class DroidHealth : MonoBehaviour
{
    public bool BeingDestroyed;
    public GameObject ParentObject;
    public float DroidDestroyTime;
    public Renderer droidMaterial;
    public GameObject HealthPowerUp;
    public GameObject ShieldPowerUp;
    public Droid MainScript;

    void Update()
    {
        if(DroidDestroyTime > 0.55)
        {
            KillDroid();
        }
        if(BeingDestroyed == true)
        {
            DroidDestructionVFX();
        }
        if(Keyboard.current.spaceKey.isPressed)
        {
            SpawnPowerUp("Shield");
        }
    }
    /// <summary>
    /// Runs deathTime code
    /// </summary>
    public void KillDroid()
    {
        DroidController.Instance.RecordDroidDeath();
        CheckIfDropsPowerUp();
        OffScreenIndicator.Instance.RemoveDroid(ParentObject);
        Destroy(ParentObject);
    }
    /// <summary>
    /// Records that droids been hit
    /// </summary>
    public void ApplyDamage()
    {
        BeingDestroyed = true;
        MainScript.CanShoot = false;
    }
    /// <summary>
    /// Handes death VFX
    /// </summary>
    private void DroidDestructionVFX()
    {
        DroidDestroyTime += Time.deltaTime / 2;
        droidMaterial.material.SetFloat("_dissolveAmount", 0.05f + DroidDestroyTime);
    }
    /// <summary>
    /// Handles powerUp drop
    /// </summary>
    private void CheckIfDropsPowerUp()
    {
        int chance = Random.Range(0, 100);
        if(chance >= 95)
        {
            SpawnPowerUp("Shield");
        }
        else if(chance < 95 && chance >= 90)
        {
            SpawnPowerUp("Health");
        }
    }
    /// <summary>
    /// Spawns power-up
    /// </summary>
    /// <param name="PowerUptype">Type of power-up to spawn</param>
    private void SpawnPowerUp(string PowerUptype)
    {
        if(PowerUptype == "Shield")
        {
            GameObject powerUp = Instantiate(ShieldPowerUp);
            powerUp.transform.position = new Vector3() {y = 0, x = transform.position.x, z = transform.position.z};
        } 
        else if(PowerUptype == "Health")
        {
            GameObject powerUp = Instantiate(HealthPowerUp);
            powerUp.transform.position = new Vector3() {y = 0, x = transform.position.x, z = transform.position.z};
        }
    }
}