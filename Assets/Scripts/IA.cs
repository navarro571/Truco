using Assets.Scripts.Controllers;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour, IJugador
{
    public GameObject cartasGameObject;
    public List<Carta> cartasDisponibles;
    public Carta[] cartas;
    public bool MiTurno { get; set; }
    public Transform CartaPosicionFinal { get; set; }
    public bool Flor { get { return _flor; } }

    private GameController _gameController;
    private bool _validarCanto;
    private bool _flor;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        CartaPosicionFinal = _gameController.CartaPosicionBot;
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
        this.cartasDisponibles = new List<Carta>(cartas.GetComponentsInChildren<Carta>());
    }

    bool IJugador.ValidarCanto()
    {
        _validarCanto = false;
        InfoCartas cartaInfo = HelperFunctions.ObtenerPiezas(cartas);
        if (cartaInfo.flor) {
            Debug.Log("FLOR");
            ActionController.Accion(gameObject, eAction.florCanto);
        }
        return cartaInfo.flor;
    }

    private void CambioTurnoEvent(int jugadorTurnoID)
    {
        if (jugadorTurnoID == gameObject.GetInstanceID())
        {
            Debug.Log("TURNO IA");
            if (_validarCanto)
            {
                _flor = ((IJugador)this).ValidarCanto();
            }
            //IAAction.Invoke(eTipoAccion.RESPUESTA, eAction.Envido);
            JugarCarta(UnityEngine.Random.Range(0, cartasDisponibles.Count - 1));
        }
    }

    public void CartaJugada(Carta carta) => _gameController.JugarCarta(carta, false);

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

    public void OnAction(GameObject gameObject, eAction tipoAccion)
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
                ActionController.Respuesta(gameObject, ValidarPosibleTruco());
                break;
            case eAction.Retruco:
                break;
            case eAction.ValeCuatro:
                break;
        }
        //bool flor = _flor;
        //if(_validarCanto)
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
        //            return ValidarPosibleEnvido();
        //        case eAction.RealEnvido:
        //            break;
        //        case eAction.FaltaEnvido:
        //            break;
        //        case eAction.Truco:
        //            return ValidarPosibleTruco();
        //        case eAction.Retruco:
        //            return ValidarPosibleRetruco();
        //        case eAction.ValeCuatro:
        //            return ValidarPosibleTrucoValeCuatro();
        //    }
        //    return false;
        //}

    }

    private bool ValidarPosibleEnvido()
    {
        return true;
    }
    private bool ValidarPosibleTruco()
    {
        return true;
    }
    private bool ValidarPosibleRetruco()
    {
        return true;
    }
    private bool ValidarPosibleTrucoValeCuatro()
    {
        return true;
    }
}
