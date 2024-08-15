using UnityEngine;

public class LightSaberAudioManager : MonoBehaviour
{
    public AudioSource lightSaberAudioSource;
    private Vector3 velocity;
    public AudioClip lightSaberMovingSound;
    public AudioClip lightSaberHum;
    private Vector3 lastPosition;
    public GameObject lightSaber;
    private float speed;
    void Awake()
    {
        lightSaberAudioSource.spatialBlend = 1;
    }


    private void FixedUpdate()
    {
        speed = Vector3.Distance(lastPosition, lightSaber.transform.position) / Time.deltaTime;
        lastPosition = lightSaber.transform.position;

    }
    
    void Update()
    {
        if (speed > 0.5)
        {
            lightSaberAudioSource.PlayOneShot(lightSaberMovingSound);
        }
        else if (lightSaberAudioSource.isPlaying == false)
        {
            lightSaberAudioSource.PlayOneShot(lightSaberHum);
        }
    }
}
