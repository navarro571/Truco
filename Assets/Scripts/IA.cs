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
        //Subscribir a evento inbound
        //Subscribir a evento turno
    }

    private void OnDisable()
    {
        GameController.CambioTurno -= CambioTurnoEvent;
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
            IAAction.Invoke(eTipoAccion.RESPUESTA, eEstrategia.florCanto);
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
            //IAAction.Invoke(eTipoAccion.RESPUESTA, eEstrategia.Envido);
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

    public bool AccionEntrante(eEstrategia tipoAccion)
    {
        bool flor = _flor;
        if(_validarCanto)
        {
            _flor = ((IJugador)this).ValidarCanto();
            flor = _flor;
        }
        if (flor)
        {
            return false;
        } 
        else
        {
            switch (tipoAccion)
            {
                case eEstrategia.flor:
                    break;
                case eEstrategia.florEnvido:
                    break;
                case eEstrategia.florResto:
                    break;
                case eEstrategia.Envido:
                    return ValidarPosibleEnvido();
                case eEstrategia.RealEnvido:
                    break;
                case eEstrategia.FaltaEnvido:
                    break;
                case eEstrategia.Truco:
                    break;
                case eEstrategia.Retruco:
                    break;
                case eEstrategia.ValeCuatro:
                    break;
            }
            return false;
        }
        
    }

    private bool ValidarPosibleEnvido()
    {
        return true;
    }
}
