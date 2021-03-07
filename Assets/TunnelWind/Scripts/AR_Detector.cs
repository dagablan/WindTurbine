using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_Detector : MonoBehaviour
{

    //	Definimos los posibles estados. Inicializando, y buscando o encontrado
    public enum FocusState { Inicializando, Escaneando, Detectado }
    private FocusState squareState;
    public FocusState SquareState
    {
        get
        {
            return squareState;
        }
        set
        {
            squareState = value;
        }
    }

    //	Este será el controlador principal de ARFoundation
    [Space]
    [Header("Deteccion de planos")]
    public ARRaycastManager raycastManager;
    //	donde almacenaremso los rayos que trazamos
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    bool first_time = true;

    //	Inicializamos la instancia y la variable ARSession (plugin AR)
    void Awake()
    {
        if (raycastManager == null) raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!first_time) return;

        //	Tiramos una linea desde el centro de la pantalla
        //	Comprobamos si esa linea choca contra un plano de AR

        //	Deteccion
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, .2f);

        //	Si choca estamos apuntando bien, podemos realizar acciones
        if (raycastManager.Raycast(center, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            SquareState = FocusState.Detectado;

            first_time = false;
            ShowAlert();
        }
        else
        {
            //	Si no hay que seguir buscando
            SquareState = FocusState.Escaneando;
        }
    }


    public GameObject alert_object;

    //	Mostramos una alerta cuando lo hayamos detectado
    public void ShowAlert()
    {
        CancelInvoke("HideAlert");
        if (alert_object != null) alert_object.SetActive(true);
        Invoke("HideAlert", 2f);
    }

    void HideAlert()
    {
        if (alert_object != null) alert_object.SetActive(false);
    }
}