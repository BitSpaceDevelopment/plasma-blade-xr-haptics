using UnityEngine;

public class DissolveAfterTime : MonoBehaviour
{
    public float TimeToStartDissolveInSeconds = 1;
    public float timer = 0;
    public float DissolveTime = 0;
    public Material DissolveMaterial;
    public bool IsDissolving = false;
    // Start is called before the first frame update
    void Start()
    {
        DissolveMaterial = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDissolving)
        {
            Dissolve();
            if(DissolveTime >= 1.10)
            {
                Destroy(gameObject);
            }
        } 
        else
        {   
            if(timer >= TimeToStartDissolveInSeconds)
            {
                IsDissolving = true;
            }
            timer += Time.deltaTime;
        }
    }
    /// <summary>
    /// Dissolved objects material
    /// </summary>
    private void Dissolve()
    {
        DissolveTime += Time.deltaTime;
        DissolveMaterial.SetFloat("_dissolveAmount", 0.05f + DissolveTime);
    }
}
