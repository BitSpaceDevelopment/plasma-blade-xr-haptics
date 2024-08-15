using System.Collections;
using UnityEngine;

public class ShootVFXHandler : MonoBehaviour
{
    public ParticleSystem muzzle;
    public ParticleSystem beam;
    public ParticleSystem smoke;
    public ParticleSystem particle;
    public ParticleSystem WarmUp;

    public bool hasSmoke = true;
    public bool hasMuzzle = true;
    public bool hasParticle = true;

    public float vfxSeconds = 1;
    public void StartWarmUp()
    {
        StartCoroutine(PlayWarmUpVFX());
    }
    public void StartVFX()
    {
        StartCoroutine(PlayVFX());
    }
  
    IEnumerator PlayWarmUpVFX()
    {
        WarmUp.Play();
        yield return new WaitForSeconds(vfxSeconds);
    }
    IEnumerator PlayVFX()
    {
        beam.Play();
        if (hasSmoke)
        {
            smoke.Play();
        }
        if (hasMuzzle)
        {
            muzzle.Play();
        }
        if(hasParticle) 
        { 
            particle.Play();
        }

        yield return new WaitForSeconds(vfxSeconds);
    }
}
