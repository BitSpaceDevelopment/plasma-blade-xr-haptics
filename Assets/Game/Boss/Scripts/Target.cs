using UnityEngine;

public class Target : MonoBehaviour
{
    public static Target Instance { get; private set; }

    private float _size = 10;
    private float _speed = 10;
    public Rigidbody Rb;
    public Transform Tr;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Time.timeScale = 0.5F;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
    void Update() {
        var dir = new Vector3(Mathf.Cos(Time.time * _speed) * _size, Mathf.Sin(Time.time * _speed) * _size);

        Rb.velocity = dir;
    }

    public void Explode() => Destroy(gameObject);
}
