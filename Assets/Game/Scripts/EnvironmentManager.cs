using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject[] titanHapticLogos;
    public GameObject[] walls;
    public GameObject BossSpawner;
    public Transform worldFloor;

    private Vector3[] wallPoints;
    private Vector3 boundaryFloor;

    private float timeTick = 0.25f;
    private float time = 0f;

    private void Awake()
    {
        OVRManager.InputFocusAcquired += OnInputFocusAcquired;
    }

    private void Start()
    {
        SetNewBoundary();
    }

    private void OnInputFocusAcquired()
    {
        SetNewBoundary();
    }

    #region BoundaryMath
    public float FloatMidPoint(float a, float b)
    {
        return (a + b) / 2;
    }

    public Vector3 VectorMidPoint(Vector3 a, Vector3 b)
    {
        float x = (a.x + b.x) / 2;
        float y = (a.y + b.y) / 2;
        float z = (a.z + b.z) / 2;

        return new Vector3(x, y, z);
    }
    #endregion

    #region Boundary
    public void CenterPlayer()
    {
        wallPoints = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

        Vector3 pointA = VectorMidPoint(wallPoints[1], wallPoints[2]);
        Vector3 pointB = VectorMidPoint(wallPoints[3], wallPoints[0]);

        Vector3 between = pointB - pointA;

        float distance = between.magnitude;

        worldFloor.transform.position = pointA + (between / 2.0f);
        worldFloor.transform.LookAt(pointA);
    }


    public void SetNewBoundary()
    {
        boundaryFloor = OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea);


        boundaryFloor = new Vector3(boundaryFloor.x, 0.025f, boundaryFloor.z);
        worldFloor.localScale = boundaryFloor;


        titanHapticLogos[0].transform.localPosition = new Vector3(worldFloor.localScale.x / 2, 1, (worldFloor.localScale.z / 2) - (worldFloor.localScale.z / 2));
        titanHapticLogos[1].transform.localPosition = new Vector3(-worldFloor.localScale.x / 2, 1, (worldFloor.localScale.z / 2) - (worldFloor.localScale.z / 2));
        titanHapticLogos[2].transform.localPosition = new Vector3((worldFloor.localScale.x / 2) - (worldFloor.localScale.x / 2), 1, -worldFloor.localScale.z / 2);
        titanHapticLogos[3].transform.localPosition = new Vector3((worldFloor.localScale.x / 2) - (worldFloor.localScale.x / 2), 1, worldFloor.localScale.z / 2);

        BossSpawner.transform.localPosition = new Vector3(titanHapticLogos[0].transform.localPosition.x, 0, titanHapticLogos[0].transform.localPosition.z);
        
        for (int i = 0; i < titanHapticLogos.Length; i++)
        {
            walls[i].transform.position = titanHapticLogos[i].transform.position;
        }
        float length = titanHapticLogos[0].transform.localPosition.x * 2;
        float width = titanHapticLogos[3].transform.localPosition.z * 2;


        walls[0].transform.localScale = new Vector3(width, 2, 0.01f);
        walls[1].transform.localScale = new Vector3(width, 2, 0.01f);
        walls[2].transform.localScale = new Vector3(length, 2, 0.01f);
        walls[3].transform.localScale = new Vector3(length, 2, 0.01f);


        CenterPlayer();
    }
    #endregion
}
