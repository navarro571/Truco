using Assets.Scripts.Controllers;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Jugador : MonoBehaviour, IJugador
{
    public GameObject cartasGameObject;
    public Carta[] cartas;
    public bool MiTurno { get; set; }
    public Transform CartaPosicionFinal { get; set; }
    public bool Flor { get { return _flor; } }

    private GameController _gameController;
    private PanelAcciones _panelAcciones;
    private bool _validarCanto;
    private bool _flor;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        _panelAcciones = FindObjectOfType<PanelAcciones>();
        CartaPosicionFinal = _gameController.CartaPosicionJugador;
    }

    private void OnEnable()
    {
        GameController.CambioTurno += CambioTurnoEvent;
        ActionController.OnAction += OnAction;
        //Subscribir a evento inbound
        //Subscribir a evento turno
    }

    private void OnDisable()
    {
        GameController.CambioTurno -= CambioTurnoEvent;
        ActionController.OnAction -= OnAction;
        //Desubscribir a evento inbound
        //Desubscribir a evento turno
    }

    public void Reset()
    {
        _validarCanto = true;
        _flor = false;
    }

    public void DarCartas(GameObject cartas)
    {
        this.cartasGameObject = cartas;
        this.cartas = cartas.GetComponentsInChildren<Carta>();
        ((IJugador)this).ValidarCanto();
    }

    bool IJugador.ValidarCanto()
    {
        _validarCanto = false;
        InfoCartas cartaInfo = HelperFunctions.ObtenerPiezas(cartas);
        if (cartaInfo.flor)
        {
            Debug.Log("FLOR");
            ActionController.Accion(gameObject, eAction.florCanto);
        }

        return cartaInfo.flor;
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
            if (_validarCanto)
            {
                _flor = ((IJugador)this).ValidarCanto();
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
            _gameController.JugarCarta(carta, true);
        }
    }

    public void Accion(eAction tipo) => ActionController.Accion(gameObject, tipo);

    public async void OnAction(GameObject gameObject, eAction tipoAccion)
    {
        bool flor = false;
        if (_validarCanto)
        {
            _flor = ((IJugador)this).ValidarCanto();
            flor = _flor;
        }
        switch (tipoAccion)
        {
            case eAction.florCanto:
                //si tiene flor entonces ver si se achica o va para delante
                if (!flor) ActionController.Respuesta(gameObject, false);
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
                bool trucoRespuesta = await _panelAcciones.PanelRespuesta("Truco");
                ActionController.Respuesta(gameObject, trucoRespuesta);
                break;
            case eAction.Retruco:
                break;
            case eAction.ValeCuatro:
                break;
        }
        //bool flor = _flor;
        //if (_validarCanto)
        //{
        //    _flor = ((IJugador)this).ValidarCanto();
        //    flor = _flor;
        //}
        //if (flor)
        //{
        //    return false;
        //}
        //else
        //{
        //    switch (tipoAccion)
        //    {
        //        case eAction.flor:
        //            break;
        //        case eAction.florEnvido:
        //            break;
        //        case eAction.florResto:
        //            break;
        //        case eAction.Envido:
        //            return await _panelAcciones.PanelRespuesta("Envido");
        //        case eAction.RealEnvido:
        //            break;
        //        case eAction.FaltaEnvido:
        //            break;
        //        case eAction.Truco:
        //            break;
        //        case eAction.Retruco:
        //            break;
        //        case eAction.ValeCuatro:
        //            break;
        //    }
        //    return false;
        //}
    }
}
