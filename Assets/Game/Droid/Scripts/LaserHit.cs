using System.Collections;
using UnityEngine;

public class LaserHit : MonoBehaviour
{
    public ParticleSystem Particles;
    public ParticleSystem Smoke;
    public ParticleSystem Beam;
    
    public void SetHit(Transform bulletTransfrom, Transform wallTransfrom, bool isWall)
    {
        transform.position = bulletTransfrom.position;
        if (isWall) 
        {
            transform.eulerAngles = new Vector3(90, wallTransfrom.eulerAngles.y, 0);
        }
        PlayHitVFX();
    }

    public void PlayHitVFX()
    {
        Particles.Play();
        Smoke.Play();
        Beam.Play();

        StartCoroutine(TimedVFX());
    }

    public IEnumerator TimedVFX()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
