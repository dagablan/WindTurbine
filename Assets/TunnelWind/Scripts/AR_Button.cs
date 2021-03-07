using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AR_Button : MonoBehaviour
{

    private Image buttonImage;
    [SerializeField] private int index = 0;
    //[SerializeField] private Sprite active_sprite;
    //[SerializeField] private Sprite inactive_sprite;

    void Start()
    {
        buttonImage = this.GetComponent<Image>();
    }

    //	Comprobamos si es el seleccionado o no y ponemos la imagen correcta
    //void Update()
    //{
    //    if (AR_Manager.Instance != null && AR_Manager.Instance.selectedIndex == index) buttonImage.sprite = active_sprite;
    //    else buttonImage.sprite = inactive_sprite;
    //}

    //	al hacer click lo asiganmos en el AR Manager
    public void OnButtonClick()
    {
        if (AR_Manager.Instance != null) AR_Manager.Instance.SetSelectedIndex(index);
    }
}
