using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [SerializeField] private CameraRotation cameraRotation;
    [SerializeField] private ActiveVectors activeVectors;   

    public CameraRotation CameraRotation => cameraRotation;

    private GameObject followTarget;                        
    private Vector3 offset;                                 
    private Vector3 changePos;                              

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SetTarget(GameObject target)
    {
        followTarget = target;                                          
        offset = followTarget.transform.position - transform.position;  
        changePos = transform.position;                                 
    }

    private void LateUpdate()
    {
        if (followTarget)                                               
        {
            if (activeVectors.x)                                        
            {                                                           
                changePos.x = followTarget.transform.position.x - offset.x;
            }
            if (activeVectors.y)                                        
            {                                                           
                changePos.y = followTarget.transform.position.y - offset.y;
            }
            if (activeVectors.z)                                        
            {                                                           
                changePos.z = followTarget.transform.position.z - offset.z;
            }
            transform.position = changePos;                             
        }
    }
}

[System.Serializable]
public class ActiveVectors
{
    public bool x, y, z;
}
