using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public static event Action<GameObject, eAction> OnAction;
    public static event Action<bool> OnWaitingStateChange;

    private static void SetEsperando(bool value)
    {
        _esperando = value;
        OnWaitingStateChange?.Invoke(_esperando);
    }

    private static Jugador ObtenerJugador()
    {
        return _jugador != null ? _jugador : FindObjectOfType<Jugador>();
    }
    private static GameController ObtenerGameController()
    {
        return _gameController != null ? _gameController : FindObjectOfType<GameController>();
    }

    private static int _trucoNivel;
    private static (GameObject, eAction) ultimaAccion;
    private static bool _esperando = false;
    private static Jugador _jugador;
    private static GameController _gameController;

    public static eAction ObtenerUltimaAccion()
    {
        return ultimaAccion.Item2;
    }

    public static void Accion(GameObject jugador, eAction estrategia)
    {
        SetEsperando(true);
        ultimaAccion = (jugador, estrategia);
        OnAction?.Invoke(jugador, estrategia);
    }

    public static void Respuesta(GameObject jugador, bool respuesta)
    {
        Jugador jugadorLocal = ObtenerJugador();
        GameController gameController = ObtenerGameController();
        switch (ultimaAccion.Item2)
        {
            case eAction.florCanto:
                if (!respuesta)
                {
                    if (jugadorLocal.gameObject.GetInstanceID() == ultimaAccion.Item1.GetInstanceID())
                        gameController.SumarPuntosEquipoA(3);
                    else
                        gameController.SumarPuntosEquipoB(3);
                }
                break;
            case eAction.flor:
                break;
            case eAction.florEnvido:
                break;
            case eAction.florResto:
                break;
            case eAction.Envido:

                break;
            case eAction.RealEnvido:
                break;
            case eAction.FaltaEnvido:
                break;
            case eAction.Truco:
                break;
            case eAction.Retruco:
                break;
            case eAction.ValeCuatro:
                break;
        }
    }
}
