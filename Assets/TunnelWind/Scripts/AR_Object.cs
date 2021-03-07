using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AR_Object : MonoBehaviour
{
    public SphereController sphereController;

    void Start()
    {
        sphereController = gameObject.GetComponent<SphereController>();
        if (sphereController == null) sphereController = gameObject.AddComponent<SphereController>();
        sphereController.locked = false;
    }

    void OnMouseDown()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log("Touched the UI");
                return;
            }

            Debug.Log("Selected: " + this.gameObject.name);

            if (AR_Manager.Instance != null && AR_Manager.Instance.editMode != EditionMode.SELECTED) AR_Manager.Instance.SetSelectedObject(this);
        }
    }

}
