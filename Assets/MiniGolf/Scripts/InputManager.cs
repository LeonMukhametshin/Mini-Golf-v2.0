using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float distanceLimit = 1.5f;

    private float dragDistance;
    private bool canRotate = false;

    private void Update()
    {
        if (GameManager.singleton.gameStatus != GameStatus.Playing) return;

        if (Input.GetMouseButtonDown(0) && !canRotate)
        {
            GetDistance();
            canRotate = true;

            if (dragDistance <= distanceLimit)
            {
                BallController.instance.MouseDownMethod();
            }
        }

        if (canRotate)
        {
            if (Input.GetMouseButton(0))
            {

                if (dragDistance <= distanceLimit)
                {
                    BallController.instance.MouseNormalMethod();
                }
                else
                {
                    CameraFollow.instance.CameraRotation.RotateCamera(Input.GetAxis("Mouse X"));
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                canRotate = false;

                if (dragDistance <= distanceLimit) BallController.instance.MouseUpMethod();
            } 
        }
    }

    private void GetDistance()
    {
        var plane = new Plane(Camera.main.transform.forward, BallController.instance.transform.position);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            var vector3Position = ray.GetPoint(distance);
            dragDistance = Vector3.Distance(vector3Position, BallController.instance.transform.position);
        }
    }
}