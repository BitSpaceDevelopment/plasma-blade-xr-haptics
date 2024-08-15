using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public Rigidbody Rb;
    public GameObject ExplosionPrefab;

    public float Speed = 15;
    public float RotateSpeed = 150;

    public float Lifespan = 10f;
    private float _maxDistancePredict = 100;
    private float _minDistancePredict = 5;
    private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;
    public OVRGrabbable GrabbableScript;
    public bool HasBeenGrabbed;

    private float _deviationAmount = 50;
    private float _deviationSpeed = 2;

    private void FixedUpdate() {
        if(!GrabbableScript.isGrabbed && !HasBeenGrabbed)
        {
            Rb.velocity = transform.forward * Speed;

            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, Target.Instance.Tr.position));

            PredictMovement(leadTimePercentage);

            AddDeviation(leadTimePercentage);

            RotateRocket();
        }
        else if(GrabbableScript.isGrabbed )
        {
            Rb.isKinematic = true;
            HasBeenGrabbed = true;
        } 
        else
        {
            Rb.isKinematic = false;
            Rb.angularVelocity = Vector3.zero;
            Rb.velocity = transform.forward * Speed;
        }   
    }

    void Start()
    {
        DestroyAfterDelay();
    }
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Boss" && HasBeenGrabbed == true && !GrabbableScript.isGrabbed)
        {
            DroidBoss.Instance.TakeDamage();
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Player" && HasBeenGrabbed == false && !GrabbableScript.isGrabbed)
        {
            Player.Instance.ApplyDamage(4);
            Destroy(gameObject);
        } 
    }
    /// <summary>
    /// Estimates where player will be based of of velocity
    /// </summary>
    private void PredictMovement(float leadTimePercentage) {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = Target.Instance.Rb.position + Target.Instance.Rb.velocity * predictionTime;
    }
    /// <summary>
    /// Makes rockets movement more unpredictable
    /// </summary>
    private void AddDeviation(float leadTimePercentage) {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
        
        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }
    /// <summary>
    /// Has rocket move based off of predicated movement and deviation
    /// </summary>
    private void RotateRocket() {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        Rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, RotateSpeed * Time.deltaTime));
    }
   IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(Lifespan);
        Destroy(gameObject);
    }
}

