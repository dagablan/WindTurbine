using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour
{

    /*	Movement input	*/
    /*	proporciones de giro	*/
    public static float _rotationDelta = 5.00f;
    private static Vector3 _actionRotation;
    private static Vector3 _aimRotation;

    /*Proporciones de movimiento*/
    public static float _movementDelta = 2f;
    private static Vector2 _actionMovement;
    private static Vector2 _aimMovement;

    private static double _movementVariation = 0.0;
    private static double _anguloVertical = 0.0;
    /*	proporciones de giro	*/

    public Transform thisTransform;
    public bool locked = true;
    public static Quaternion initialRotation = Quaternion.identity;

    // Use this for initialization
    void Start()
    {
        if (thisTransform == null) thisTransform = transform;
        //Debug.LogError("INICIAL ROTATION" + _aimRotation);
        //Debug.LogError("ACTION ROTATION" + _actionRotation);
        initialRotation = thisTransform.rotation;
    }

    bool first_touch = true;
    // Update is called once per frame
    void OnGUI()
    {

        if (locked)
        {
            first_touch = true;
            return;
        }

        if ((Input.touchCount == 2) && AR_Manager.Instance.selectedObject.gameObject == gameObject)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _actionRotation = (Input.GetTouch(0).position);
            }
            else
            {
                if (_actionRotation.magnitude != 0) _anguloVertical = -((_aimRotation.x - _actionRotation.x) * _rotationDelta);

                _anguloVertical = -(((Input.GetTouch(0).position).x - _actionRotation.x) * _rotationDelta);
                _actionRotation = (Input.GetTouch(0).position);

                Vector3 targetRotation = new Vector3((float)Radianes(0), (float)Radianes(_anguloVertical), 0);
                thisTransform.Rotate(targetRotation, Space.Self);
            }

        }
        else if (Input.touchCount == 1 && AR_Manager.Instance.selectedObject.gameObject == gameObject)
        {
            thisTransform.position = new Vector3(
                (thisTransform.position.x + Input.GetTouch(0).deltaPosition.x * 0.0005f),
                thisTransform.position.y,
                (thisTransform.position.z + Input.GetTouch(0).deltaPosition.y * 0.0005f));
        }
    }

    public void Restablecer()
    {
        if (thisTransform != null)
        {
            thisTransform.position = Vector3.zero;
            thisTransform.rotation = initialRotation;
        }
    }

    double Radianes(double angulo)
    {
        return (double)(angulo * 2 * Mathf.PI / 360.0);
    }
}
