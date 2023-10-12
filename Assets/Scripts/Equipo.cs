using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eEquipo
{
    EquipoA,
    EquipoB,
    EquipoC
}

public class Equipo : MonoBehaviour
{
    public GameObject[] Jugadores;
    public int Puntos;

    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
}
