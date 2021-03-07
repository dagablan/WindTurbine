using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//	Vamos a tener dos modos
//	AR --> escanearemos la sala para crear objetos
//	SELECTED --> tenemos un objeto seleccionado y podemos rotarlo o eliminarlo
public enum EditionMode
{
    AR, SELECTED, PLACED, NONE
}

public class AR_Manager : MonoBehaviour
{

    public AR_PlaneHitDetection planeHitDetection;
    public Text textEnergy;
    //	Objeto actual
    public AR_Object selectedObject;

    public int selectedIndex = -1;
    public GameObject[] prefabs;

    public Transform ARCamera;
    public EditionMode editMode = EditionMode.AR;
    public bool control_xr_controller = true;

    public GameObject canvas_ar;
    public GameObject canvas_simulacion;
    public Slider sliderVelocity;
    GameObject solverEmitter;
    public GameObject canvas_selected;

    public static AR_Manager Instance = null;
    void Awake()
    {
        Instance = this;
        editMode = EditionMode.AR;
    }

    //	Hacemos que la pantalla no se bloquee sola
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {

        switch (editMode)
        {

            case EditionMode.AR:

                if (control_xr_controller)
                {
                    //	Si tenemos seleccionado un objeto valido para crear activamos el controller para crear objetos nuevos
                    if (selectedIndex >= 0 && selectedIndex < prefabs.Length)
                    {
                        AR_PlaneHitDetection.Instance.xr_active = true;
                        if (planeHitDetection != null) planeHitDetection.SetPlanesVisibilityTo(true);
                    }
                    else
                    {
                        AR_PlaneHitDetection.Instance.xr_active = false;
                        if (planeHitDetection != null) planeHitDetection.SetPlanesVisibilityTo(false);
                    }
                }

                //	Cuando estamos en modo AR no deberiamos tener ningun objeto AR seleccionado, si lo hubiera lo quitamos
                if (selectedObject) selectedObject.sphereController.locked = true;
                if (canvas_ar != null) canvas_ar.SetActive(true);

                break;
            case EditionMode.PLACED:
                //	No queremos crear objetos asi que desactivamos el AR_Controller
                AR_PlaneHitDetection.Instance.xr_active = false;

                if (canvas_simulacion != null) canvas_simulacion.SetActive(true);
                if (planeHitDetection != null) planeHitDetection.SetPlanesVisibilityTo(false);
                if (canvas_ar != null) canvas_ar.SetActive(false);
                if (canvas_selected != null) canvas_selected.SetActive(false);

                sliderVelocity.onValueChanged.AddListener(WindPower.Instance.ChangeVelocityWind);

                break;

            case EditionMode.SELECTED:

                AR_PlaneHitDetection.Instance.xr_active = false;
                //	Si hay un objeto seleccionado le quitamos el bloqueo para poder rotarlo
                if (selectedObject) selectedObject.sphereController.locked = false;
                //	Activamos la interfaz de edicion con boton de cancelar y borrar
                if (canvas_selected != null) canvas_selected.SetActive(true);
                if (canvas_ar != null) canvas_ar.SetActive(false);
                if (planeHitDetection != null) planeHitDetection.SetPlanesVisibilityTo(false);
                if (canvas_simulacion != null) canvas_simulacion.SetActive(false);
                Debug.LogError("Objeto seleccionado: " + selectedObject.gameObject.name);
                break;

            //	Si llegamos aqui algo esta mal. Reseteamos y volvemos a modo AR
            default:
                if (control_xr_controller) AR_PlaneHitDetection.Instance.xr_active = false;
                if (selectedObject) selectedObject.sphereController.locked = true;
                Reset();
                editMode = EditionMode.AR;
                break;

        }

    }

    /*	HIT EVENT	 */

    public void XRSpawner(Vector3 arPosition, Quaternion arRotation)
    {
        Resources.UnloadUnusedAssets();

        if (selectedIndex < 0 || selectedIndex >= prefabs.Length)
        {
            return;
        }

        //	Creamos el objeto del tipo elegido y lo colocamos en el punto que tocamos el plano
        GameObject newObject = Instantiate(prefabs[selectedIndex], arPosition, arRotation) as GameObject;
        newObject.transform.rotation = arRotation;

        //	Deseleccionamos el tipo de elemento a crear para no crear mas de un elemento accidentalmente
        selectedIndex = -1;

        //Pasamos al estado colocado
        editMode = EditionMode.PLACED;

        //	Limpia la memoria
        Resources.UnloadUnusedAssets();
    }

    public void SetSelectedIndex(int index)
    {
        selectedIndex = index;
    }

    public void SetSelectedObject(AR_Object xr_object)
    {
        selectedObject = xr_object;
        editMode = EditionMode.SELECTED;
    }
    //	CAMBIO DE MODOS

    //	EDICION DE AR_Objects
    public void OnButtonDeleteClicked()
    {
        if (selectedObject == null) return;

        Destroy(selectedObject.gameObject);
        selectedObject = null;
        Reset();
    }

    public void OnButtonOFFClicked()
    {
        solverEmitter = GameObject.FindGameObjectWithTag("SolverEmitter");
        if (solverEmitter != null)
        {
            solverEmitter.gameObject.SetActive(false);
        }
    }

    public void OnButtonONClicked()
    {
        if (solverEmitter != null)
        {
            solverEmitter.gameObject.SetActive(true);
        }
    }

    public void HideInfo()
    {
        var canvas = GameObject.Find("CanvasInfo");

        canvas.SetActive(false);
    }

    public void OnButtonResetClicked()
    {
        Reset();
    }

    private void Reset()
    {
        if (selectedObject != null) selectedObject.sphereController.locked = true;
        selectedObject = null;
        selectedIndex = -1;
        editMode = EditionMode.PLACED;
    }
    //	EDICION DE AR_Objects

    public void ShowText(string message)
    {
        textEnergy.text = message;
    }


}
