using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform player;  
    public float distance = 3f;  
    public float height = 2f;  

    void Update()
    {
        Vector3 targetPosition = player.position + player.forward * distance + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.unscaledDeltaTime * 5f);
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f; 
        Quaternion rotation = Quaternion.LookRotation(-lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.unscaledDeltaTime * 5f);
    }
}
