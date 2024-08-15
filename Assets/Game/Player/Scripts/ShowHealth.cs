using UnityEngine;

public class ShowHealth : MonoBehaviour
{
    public GameObject healthBar;
    public float downwardAngleThreshold = 45f; 

    void Update()
    {
        Quaternion rotation = transform.rotation;
        float angleDownward = Vector3.Angle(transform.up, Vector3.down);

        bool isAngleDownward = angleDownward <= downwardAngleThreshold;

        if (!isAngleDownward)
        {
            healthBar.SetActive(true);
        }
        else
        {
            healthBar.SetActive(false);
        }
        healthBar.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
    }
}
