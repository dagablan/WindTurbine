using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using System;
using UnityEngine.UI;

public class WindPower : MonoBehaviour
{
    private float velocityEmitter = 2.5f;
    private float velocityWind = 3.3f;
    float time = 0f;
    public ObiSolver solver;
    private string textoAMostrar = "";
    public ObiEmitter emitter;
    public static WindPower Instance = null;
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }
    public void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {

        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {

            if (contact.distance < 0.01)
            {
                ObiColliderBase collider = world.colliderHandles[contact.other].owner;
                if (collider != null)
                {

                    if (collider.gameObject == gameObject && velocityEmitter >= 2.2f && velocityEmitter <= 8)
                    {
                        Vector3 vectorTurbine = transform.forward;
                        Vector3 vectorWind = emitter.transform.forward;

                        float vectorAngle = Vector3.Cross(vectorTurbine, vectorWind).magnitude;
                        float efficiency = 1 - vectorAngle;

                        PositionText(vectorTurbine, vectorWind, vectorAngle, efficiency);
                        BladeRotation();

                        if (AR_Manager.Instance != null && AR_Manager.Instance.editMode == EditionMode.SELECTED && AR_Manager.Instance.selectedObject.gameObject == gameObject)
                        {
                            double AreaWindturbine = Mathf.Pow(45, 2) * Mathf.PI;
                            double energy = (1.225f * 0.5 * Mathf.Pow(velocityWind, 3) * AreaWindturbine * efficiency) / 1000;
                            float speed = 0.9f * velocityWind + 10;

                            time = Time.deltaTime;
                            textoAMostrar += "Velocidad de giro (rpm): " + Math.Round(speed, 2) + "\n";
                            textoAMostrar += "Potencia generada (KW): " + Math.Round(energy, 2) + "\n";
                            AR_Manager.Instance.ShowText(textoAMostrar);
                        }
                    }
                    else
                    {
                        time += Time.deltaTime;
                        if (time - Time.deltaTime > 1 && AR_Manager.Instance != null && AR_Manager.Instance.editMode == EditionMode.SELECTED && AR_Manager.Instance.selectedObject.gameObject == gameObject)
                        {
                            textoAMostrar = string.Empty;
                            textoAMostrar += "Velocidad de giro (rpm): 0 \n";
                            textoAMostrar += "Potencia generada (KW): 0 \n";

                            AR_Manager.Instance.ShowText(textoAMostrar);
                        }
                    }
                }
            }
        }
    }

    private void PositionText(Vector3 vectorTurbine, Vector3 vectorWind, float vectorAngle, float efficiency)
    {
        if (Math.Round(efficiency, 2) == 1)
        {
            textoAMostrar = "Posición del aerogenerador: Óptima \n";
            Debug.Log("Está en la posición óbtima");
        }
        else if (Math.Round(efficiency, 2) == 0)
        {
            textoAMostrar = "Posición del aerogenerador: Peor posición posible respcto la fuente del viento principal \n";
            Debug.Log("Está en la peor posicion posible");
        }
        else
        {
            textoAMostrar = "Posición del aerogenerador: No es la óptima. Tiene un redimiento del " + Math.Round(efficiency, 2) * 100 + "% \n";
            Debug.Log("No está en la posicion obtima: " + vectorAngle + " vectores: " + Vector3.Cross(vectorTurbine, vectorWind));
        }
    }

    private void BladeRotation()
    {
        var rotation = 0.085f * velocityEmitter - 0.08f;
        transform.GetChild(0).Rotate(-rotation, 0, 0);
        transform.GetChild(1).Rotate(-rotation, 0, 0);
        transform.GetChild(2).Rotate(-rotation, 0, 0);
    }

    public void ChangeVelocityWind(float velocity)
    {
        if (velocity > 0.1f)
        {
            if (velocity > 10.5f)
            {
                emitter.lifespan = 0.5f;
            }
            else if (velocity > 7.5f)
            {
                emitter.lifespan = 0.8f;
            }
            else if (velocity > 6.5f)
            {
                emitter.lifespan = 1f;
            }
            else if (velocity > 4.7f)
            {
                emitter.lifespan = 1.5f;
            }
            else if (velocity > 4f)
            {
                emitter.lifespan = 1.8f;
            }
            else if (velocity > 3.7f)
            {
                emitter.lifespan = 2f;
            }
            else
            {
                emitter.lifespan = 2.2f;
            }
            velocityEmitter = 0.753f * velocity - 0.28f;
        }
        else
        {
            velocityEmitter = 0;
        }
        emitter.speed = velocityEmitter;
        velocityWind = velocity;
    }

}
