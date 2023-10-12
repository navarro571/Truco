using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAcciones : MonoBehaviour
{
    private Jugador jugador;
    private void Start()
    {
        jugador = FindObjectOfType<Jugador>();
    }

    public void Truco()
    {
        jugador.Accion(eEstrategia.Truco);
    }

    public void Envido()
    {
        jugador.Accion(eEstrategia.Envido);
    }

    public void Real()
    {
        jugador.Accion(eEstrategia.RealEnvido);
    }

    public void Falta()
    {
        jugador.Accion(eEstrategia.FaltaEnvido);
    }
}
