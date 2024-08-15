using UnityEngine;
using UnityEngine.UI;

public class HandGrabber : OVRGrabber
{
    public OVRHand hand;
    public float pinchTreshold = 0.7f;

    public Text handDetails;

    private Rigidbody rb;

    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        if(m_grabbedObj)
        {
            rb = m_grabbedObj.GetComponent<Rigidbody>();
        }
    }

    public override void Update()
    {
        base.Update();
        CheckIndexPinch();
    }
    /// <summary>
    /// Checks hand pinching strength
    /// </summary>
    public void CheckIndexPinch()
    {
        float pinchIndexStrength = GetComponent<OVRHand>().GetFingerPinchStrength(OVRHand.HandFinger.Index);

        bool isPinching = pinchIndexStrength > pinchTreshold;
    
        if (!m_grabbedObj && isPinching && m_grabCandidates.Count > 0 )
            GrabBegin();
        else if (m_grabbedObj && !isPinching)
            GrabEnd();
    }
    /// <summary>
    /// Starts grabbing nearest grabbable object
    /// </summary>
    protected override void GrabBegin()
    {
        base.GrabBegin();
        handDetails.text = transform.name;
    
        if (rb != null && rb.isKinematic == false)
        {
            rb.isKinematic = true;
        }
    }
}