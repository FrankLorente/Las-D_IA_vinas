using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCultist : Detection
{
    public Unit unit;    // nuevo

    void Update()
    {
        base.MiUpdate();

        if (Physics.CheckSphere(transform.position, sphereRadiusPatrol, layerPatrol))
        {
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(false);
            if (unit.siguienteWaypoint < unit.listaPuntos.Count - 1)
                unit.siguienteWaypoint++;
            else
                unit.siguienteWaypoint = 0;
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(true);
        }
    }
}
