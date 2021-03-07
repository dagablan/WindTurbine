using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowVelocityValue : MonoBehaviour
{
    public Text VelocityText;

    // Update is called once per frame
    public void TextUpdate(float velocidad)
    {
        VelocityText.text = System.Math.Round(velocidad, 2).ToString() + " m/s";

        if(velocidad > 11f || velocidad < 3.3f)
        {
            VelocityText.color = Color.red;
        }
        else
        {
            VelocityText.color = Color.green;
        }
    }
}
