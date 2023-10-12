using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ModoJuego", fileName = "ModoJuego")]
public class ModoJuego : ScriptableObject
{
    public int CantidadJugadoresPorEquipo;
    [Header("Posiciones")]
    public Vector3[] PosicionEquipoA;
    public Vector3[] PosicionEquipoB;
    [Header("Rotaciones")]
    public Vector3[] RotacionEquipoA;
    public Vector3[] RotacionEquipoB;
}
