using Assets.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PanelAcciones : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelAccionesDer;
    [SerializeField]
    private GameObject _panelRespuesta;
    [SerializeField]
    private Button _trucoBtn;
    [SerializeField]
    private Button _envidoBtn;
    [SerializeField]
    private Button _realEnvidoBtn;
    [SerializeField]
    private Button _faltaEnvidoBtn;

    private Jugador _jugadorInstance;
    private Jugador _jugador {
        get {
            if(_jugadorInstance == null)
            {
                _jugadorInstance = FindObjectOfType<Jugador>();
                return _jugadorInstance;
            } else {
                return _jugadorInstance;
            }
        } 
    }
    private GameController _gameController;
    private bool _waitingResponse;
    private bool _currentResponse;
    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    private void OnEnable()
    {
        Jugador.PlayerAction += PlayerAccion;
    }
    private void OnDisable()
    {
        Jugador.PlayerAction += PlayerAccion;
    }

    public void Reset()
    {
        _envidoBtn.interactable = true;
        _realEnvidoBtn.interactable = true;
        _faltaEnvidoBtn.interactable = true;
    }

    public void Truco()
    {
        _jugador.Accion(eEstrategia.Truco);
    }

    public void Envido()
    {
        _jugador.Accion(eEstrategia.Envido);
    }

    public void Real()
    {
        _jugador.Accion(eEstrategia.RealEnvido);
    }

    public void Falta()
    {
        _jugador.Accion(eEstrategia.FaltaEnvido);
    }

    public void Contar()
    {
        int contar = HelperFunctions.Contar(_jugador.cartasGameObject);
        Debug.Log("Jugador. Contar: " + contar);
    }
    public void PanelRespuestaQuiero()
    {
        _waitingResponse = false;
        _currentResponse = true;
        Debug.Log("Jugador. Quiero");
    }

    public void PanelRespuestaNoQuiero()
    {
        _waitingResponse = false;
        _currentResponse = false;
        Debug.Log("Jugador. No Quiero");
    }

    private void PlayerAccion(eTipoAccion tipo, object data)
    {
        if (tipo == eTipoAccion.RESPUESTA)
        {
            eEstrategia estrategia = (eEstrategia)data;
            if (estrategia == eEstrategia.florCanto)
            {
                _envidoBtn.interactable = false;
                _realEnvidoBtn.interactable = false;
                _faltaEnvidoBtn.interactable = false;
            }
        }
    }

    public async Task<bool> PanelRespuesta(string message) {
        _waitingResponse = true;
        _panelRespuesta.SetActive(true);
        _panelAccionesDer.SetActive(false);

        await Task.Run(async () => {
            int i = 0;
            while (_waitingResponse && i < 50)
            {
                ++i; 
                await Task.Delay(1000);
            }
            return;
        });

        _panelRespuesta.SetActive(false);
        _panelAccionesDer.SetActive(true);
        return _currentResponse;
    }
}
