using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberAudioManager : MonoBehaviour
{
    public AudioSource SaberAudioSource;
    private Vector3 velocity;
    public AudioClip SaberMovingSound;
    public AudioClip SaberHum;
    private Vector3 lastPosition;
    public GameObject Saber;
    private float speed;
    void Awake()
    {
        SaberAudioSource.spatialBlend = 1;
    }


    private void FixedUpdate()
    {
        speed = Vector3.Distance(lastPosition, Saber.transform.position) / Time.deltaTime;
        lastPosition = Saber.transform.position;

    }
    
    void Update()
    {
        if (speed > 0.5)
        {
            SaberAudioSource.PlayOneShot(SaberMovingSound);
        }
        else if (SaberAudioSource.isPlaying == false)
        {
            SaberAudioSource.PlayOneShot(SaberHum);
        }
    }
}
