using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_PlaneHitDetection : MonoBehaviour
{

    public static AR_PlaneHitDetection Instance = null;

    [Header("Instanciacion de elementos")]
    //	Aqui vamos a controlar que se active o desactive la deteccion
    public bool xr_active = true;
    bool first_touch = true;

    //	Esta es la accion que se va a ejecutar cuando hagamos click sobre un plano
    public VectorRotationEvent XRHitEvent;
    Quaternion ar_rotation = Quaternion.identity;

    [Header("Referencias ARFoundation")]
    //Referencias necesarias de ARFoundation
    public Camera xr_camera;
    public ARSessionOrigin m_SessionOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager ar_planes_manager;

    //Hits del raycast
    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    private bool trackingInitialized;

    void Awake()
    {
        Instance = this;
        if (m_SessionOrigin == null) m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    void Start()
    {
        trackingInitialized = true;
    }

    //	Aqui vamos a controlar que se active o desactive la deteccion
    public void SetXRState(bool state)
    {
        xr_active = state;
    }

    void Update()
    {

        //	Si no esta todo inicializado abortamos
        if (!xr_active || !trackingInitialized) return;

        //	Si no hay toque abortamos
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);

        //	Deteccion
        //	Lo primero comprobar que se esta comenzando el toque
        if (touch.phase == TouchPhase.Began)
        {

            //	Comprobamos que no se haya hecho click sobre la interfaz sino sobre la escena
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log("Touched the UI");
                return;
            }

            //	Tiramos una linea desde la posicion en la que tocamos la pantalla
            //	Comprobamos si esa linea choca contra un plano de AR
            if (raycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {

                //	Si es asi hemos detectado un plano y realizamos la accion que hayamos configurado
                var hitPose = s_Hits[0].pose;
                ar_rotation = hitPose.rotation;

                //	Enviamos a la accion la posicion en la que hemos tocado el plano
                ARHitPointEvent(xr_camera, hitPose.position);
            }
        }
    }

    private void ARHitPointEvent(Camera xr_camera, Vector3 arPosition)
    {

        //	Comprobamos que hemos detectado un plano de verdad, a veces se crean planos fantasma frente a la camara
        if (xr_camera != null && Mathf.Abs((xr_camera.transform.position - arPosition).magnitude) < 0.05f)
        {
            Debug.Log("<color=red>Objeto no generado. Posición incorrecta</color>");
        }
        else
        {
            //	Si todo ha ido bien realizamos la accion (se configura en la caja desde unity :) )
            if (XRHitEvent != null) XRHitEvent.Invoke(arPosition, ar_rotation);

        }
    }

    //	Establecer la visibilidad de todos los planos
    public void SetPlanesVisibilityTo(bool value)
    {
        foreach (var plane in ar_planes_manager.trackables) plane.gameObject.SetActive(value);
    }
}