using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour, IJugador
{
    public static event Action<eTipoAccion, object> PlayerAction;
    public GameObject cartasGameObject;
    public Carta[] cartas;
    private GameController gameController;
    public bool MiTurno { get; set; }
    public Transform CartaPosicionFinal { get; set; }

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        CartaPosicionFinal = gameController.CartaPosicionJugador;
    }

    private void OnEnable()
    {
        GameController.CambioTurno += CambioTurnoEvent;
        //Subscribir a evento inbound
        //Subscribir a evento turno
    }

    private void OnDisable()
    {
        GameController.CambioTurno -= CambioTurnoEvent;
        //Desubscribir a evento inbound
        //Desubscribir a evento turno
    }

    public void DarCartas(GameObject cartas)
    {
        this.cartasGameObject = cartas;
        this.cartas = cartas.GetComponentsInChildren<Carta>();
        ((IJugador)this).ValidarCartas();
    }

    void IJugador.ValidarCartas()
    {
        InfoCartas cartaInfo = HelperFunctions.ObtenerPiezas(cartas);
        if (cartaInfo.flor)
        {
            Debug.Log("FLOR");
            PlayerAction.Invoke(eTipoAccion.RESPUESTA, eEstrategia.florCanto);
        }
    }

    private void CambioTurnoEvent(int jugadorTurnoID) { 
        if(jugadorTurnoID == gameObject.GetInstanceID())
        {
            Debug.Log("TURNO JUGADOR");
            MiTurno = true;
            foreach (Carta carta in cartas)
            {
                if(!carta.CartaJugada)
                    carta.isDraggable = true;
            }
        } else {
            MiTurno = false;
            foreach (Carta carta in cartas)
            {
                carta.isDraggable = false;
            }
        }
    }

    public void CartaJugada(Carta carta)
    {
        bool cartaExiste = Array.Exists(cartas, el => el.GetInstanceID() == carta.GetInstanceID());
        if(cartaExiste)
        {
            Debug.Log("Carta Jugada: " + carta.CartaPlantilla.Numero + " De " + carta.CartaPlantilla.Tipo.ToString());
            PlayerAction?.Invoke(eTipoAccion.SINRESPUESTA, carta);
        }
    }

    public void Accion(eEstrategia tipo) => PlayerAction?.Invoke(eTipoAccion.RESPUESTA, tipo);
}
