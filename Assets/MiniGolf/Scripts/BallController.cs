using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public static BallController instance;

    [SerializeField] private LineRenderer lineRenderer;     
    [SerializeField] private float maxForce;                
    [SerializeField] private readonly float forceModifier = 0.5f;    
    [SerializeField] private GameObject areaAffector;       
    [SerializeField] private LayerMask rayLayer;            

    private float force;                                    
    private Rigidbody rigidbody;                               
    
    private Vector3 startPosition, endPosition;
    private bool canShoot = false, ballIsStatic = true;    
    private Vector3 direction;                              

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        rigidbody = GetComponent<Rigidbody>();                 
    }

    private void Update()
    {
        if (rigidbody.velocity == Vector3.zero && !ballIsStatic)   
        {
            ballIsStatic = true;                                
            LevelManager.instance.ShotTaken();                  
            rigidbody.angularVelocity = Vector3.zero;              
            areaAffector.SetActive(true);                      
        }
    }

    private void FixedUpdate()
    {
        if (canShoot)                                              
        {
            canShoot = false;                                       
            ballIsStatic = false;                                   
            direction = startPosition - endPosition;                          
            rigidbody.AddForce(direction * force, ForceMode.Impulse);  
            areaAffector.SetActive(false);                          
            UIManager.instance.PowerBar.fillAmount = 0;            
            force = 0;                                              
            startPosition = endPosition = Vector3.zero;                      
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Destroyer")                              
        {
            LevelManager.instance.LevelFailed();                    
        }
        else if (other.name == "Hole")                              
        {
            LevelManager.instance.LevelComplete();                  
        }
    }

    public void MouseDownMethod()                                           
    {
        if (!ballIsStatic) return;                                           
        startPosition = ClickedPoint();                                          
        lineRenderer.gameObject.SetActive(true);                            
        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition);  
    }

    public void MouseNormalMethod()                                         
    {
        if (!ballIsStatic) return;                                           
        endPosition = ClickedPoint();                                                
        force = Mathf.Clamp(Vector3.Distance(endPosition, startPosition) * forceModifier, 0, maxForce);   
        UIManager.instance.PowerBar.fillAmount = force / maxForce;              
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(endPosition));   
    }

    public void MouseUpMethod()
    {
        if (!ballIsStatic) return;
        canShoot = true;
        lineRenderer.gameObject.SetActive(false);
    }

    private Vector3 ClickedPoint()
    {
        Vector3 position = Vector3.zero;                                
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
        RaycastHit hit = new RaycastHit();                              

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, rayLayer))    
        {
            position = hit.point;                                       
        }
        return position;                                             
    }
}