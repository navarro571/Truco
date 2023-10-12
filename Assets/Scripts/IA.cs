using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour, IJugador
{
    public static event Action<eTipoAccion, object> IAAction;
    public GameObject cartasGameObject;
    public List<Carta> cartasDisponibles;
    public Carta[] cartas;
    private GameController gameController;

    public bool MiTurno { get; set; }
    public Transform CartaPosicionFinal { get; set; }

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        CartaPosicionFinal = gameController.CartaPosicionBot;
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
        this.cartasDisponibles = new List<Carta>(cartas.GetComponentsInChildren<Carta>());
        ((IJugador)this).ValidarCartas();
    }

    void IJugador.ValidarCartas()
    {
        InfoCartas cartaInfo = HelperFunctions.ObtenerPiezas(cartas);
        if (cartaInfo.flor) {
            Debug.Log("FLOR");
            IAAction.Invoke(eTipoAccion.RESPUESTA, eEstrategia.florCanto);
        }
    }

    private void CambioTurnoEvent(int jugadorTurnoID)
    {
        if (jugadorTurnoID == gameObject.GetInstanceID())
        {
            Debug.Log("TURNO IA");
            JugarCarta(UnityEngine.Random.Range(0, cartasDisponibles.Count - 1));
        }
    }

    public void CartaJugada(Carta carta)
    {
        IAAction?.Invoke(eTipoAccion.SINRESPUESTA, carta);
    }

    private void JugarCarta(int index)
    {
        Debug.Log("numeroCartaAJugar: " + index);
        Carta cartaAJugar = cartasDisponibles[index];
        Debug.Log("Carta Jugada: " + cartaAJugar.CartaPlantilla.Numero + " De " + cartaAJugar.CartaPlantilla.Tipo.ToString());
        cartaAJugar.Tapada = false;
        cartaAJugar.gameObject.transform.position = CartaPosicionFinal.position;
        cartaAJugar.gameObject.transform.rotation = CartaPosicionFinal.rotation;
        CartaJugada(cartaAJugar);
        cartasDisponibles.Remove(cartaAJugar);
    }
}
