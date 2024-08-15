using UnityEngine;
using EzySlice;
using Unity.XR.CoreUtils;
using System.Collections;

public class Saber : MonoBehaviour
{

    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask SliceableLayer;
    public Material CrossSectionMaterial;
    public float CutForce = 1000;
    public bool CanSlice = true;
    public ParticleSystem DroidHitVFX;

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, SliceableLayer);
        if (hasHit && hit.transform.gameObject.GetNamedChild("droid_Mesh") != null) {
            //Slice for base droids
            GameObject target = hit.transform.gameObject.GetNamedChild("droid_Mesh");
            Slice(target);
            target.GetComponent<DroidHealth>().KillDroid();
            Instantiate(DroidHitVFX, endSlicePoint.position, endSlicePoint.rotation);
        }
        else if (hasHit && CanSlice)
        {
            //slice for boss
            GameObject target = hit.transform.gameObject;
            SliceBoss(target);
            CanSlice = false;
            StartCoroutine(DelaySlice());
            if(target.tag == "Boss")
            {
                Destroy(target);
            }
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);
        if(hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, CrossSectionMaterial);
            SetupSlicedComponent(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(target, CrossSectionMaterial);
            SetupSlicedComponent(lowerHull);
        }
    }

    public void SliceBoss(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);
        if(hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, CrossSectionMaterial);
            SetupSlicedBossComponent(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(target, CrossSectionMaterial);
            SetupSlicedBossComponent(lowerHull);
        }
    }
    public void SetupSlicedBossComponent(GameObject slicedObject)
    {

        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        slicedObject.layer = 11;
        slicedObject.tag = "Boss";
        collider.convex = true;
        DroidBoss.Instance.SlicedBodyParts.Add(rb);
        rb.isKinematic = true;
    }
    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.excludeLayers = SliceableLayer;
        slicedObject.AddComponent<DissolveAfterTime>();
        collider.convex = true;
        rb.AddExplosionForce(CutForce, slicedObject.transform.position, 1);
    }
    IEnumerator DelaySlice()
    {
        yield return new WaitForSeconds(0.03f);
        CanSlice = true;
    }
}