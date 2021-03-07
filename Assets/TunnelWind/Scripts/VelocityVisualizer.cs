using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiActor))]
public class VelocityVisualizer : MonoBehaviour
{

    ObiActor actor;

    void Awake()
    {
        actor = GetComponent<ObiActor>();
    }

    void OnDrawGizmos()
    {

        if (actor == null || !actor.isLoaded)
            return;

        Gizmos.color = Color.red;
        Gizmos.matrix = actor.solver.transform.localToWorldMatrix;

        for (int i = 0; i < actor.solverIndices.Length; ++i)
        {
            int solverIndex = actor.solverIndices[i];
            Gizmos.DrawRay(actor.solver.positions[solverIndex],
                       actor.solver.velocities[solverIndex] * Time.fixedDeltaTime);
        }
    }
}