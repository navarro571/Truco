using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Jugador : MonoBehaviour, IJugador
{
    public static event Action<eTipoAccion, object> PlayerAction;
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
        ((IJugador)this).ValidarCanto();
    }

    bool IJugador.ValidarCanto()
    {
        _validarCanto = false;
        InfoCartas cartaInfo = HelperFunctions.ObtenerPiezas(cartas);
        if (cartaInfo.flor)
        {
            Debug.Log("FLOR");
            PlayerAction.Invoke(eTipoAccion.RESPUESTA, eEstrategia.florCanto);
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
            PlayerAction?.Invoke(eTipoAccion.SINRESPUESTA, carta);
        }
    }

    public void Accion(eEstrategia tipo) => PlayerAction?.Invoke(eTipoAccion.RESPUESTA, tipo);

    public async Task<bool> AccionEntrante(eEstrategia tipoAccion)
    {
        bool flor = _flor;
        if (_validarCanto)
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
                    return await _panelAcciones.PanelRespuesta("Envido");
                    break;
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
}
