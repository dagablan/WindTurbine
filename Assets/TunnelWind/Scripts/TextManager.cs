using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text texto;
    public Text TextoVelocidadAspas;

    public void WriterMessageEnergy(string message)
    {
        if(texto != null)
        {
            texto.text = message;
        }
    }

    public void WriterMessageSpeed(string message)
    {
        if (TextoVelocidadAspas)
        {
            TextoVelocidadAspas.text = message;
        }
    }
}
