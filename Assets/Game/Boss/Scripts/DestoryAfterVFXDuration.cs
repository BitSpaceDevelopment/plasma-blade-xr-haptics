using UnityEngine;

public class DestoryAfterVFXDuration : MonoBehaviour
{
    public ParticleSystem VFX;
    void Start()
    {
        Destroy(gameObject, VFX.main.duration);
    }
}
