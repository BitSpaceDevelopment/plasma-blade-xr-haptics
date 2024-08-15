using UnityEngine;

public class GlobalSoundManager : MonoBehaviour
{
    public static GlobalSoundManager instance { get; private set; }
    public AudioSource wallHitSound;
    public AudioSource playerGruntSound;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayWallHit()
    {
        wallHitSound.Play();
    }

    public void PlayPlayerOnHurt()
    {
        playerGruntSound.Play();
    }
}
