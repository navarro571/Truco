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
        GameController.CambioTurno += CambioTurno;
        ActionController.OnAction += OnAction;
        ActionController.OnWaitingStateChange += OnWaitingStateChange;
    }
    private void OnDisable()
    {
        GameController.CambioTurno += CambioTurno;
        ActionController.OnAction -= OnAction;
        ActionController.OnWaitingStateChange -= OnWaitingStateChange;
    }

    public void Reset()
    {
        _trucoBtn.interactable = true;
        _envidoBtn.interactable = true;
        _realEnvidoBtn.interactable = true;
        _faltaEnvidoBtn.interactable = true;
    }
    public void Truco()
    {
        _jugador.Accion(eAction.Truco);
        _trucoBtn.interactable = false;
    }
    public void Envido()
    {
        _jugador.Accion(eAction.Envido);
        _envidoBtn.interactable = false;
    }
    public void Real()
    {
        _jugador.Accion(eAction.RealEnvido);
        _realEnvidoBtn.interactable = false;
    }
    public void Falta()
    {
        _jugador.Accion(eAction.FaltaEnvido);
        _faltaEnvidoBtn.interactable = false;
    }
    public void Contar()
    {
        int contar = HelperFunctions.Contar(_jugador.cartasGameObject);
        Debug.Log("Jugador. Contar: " + contar);
    }

    public async Task<bool> PanelRespuesta(string message)
    {
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

    private void OnAction(GameObject jugador, eAction action)
    {
        if (_jugador.gameObject.GetInstanceID() != jugador.GetInstanceID() && action == eAction.florCanto)
        {
            _envidoBtn.interactable = false;
            _realEnvidoBtn.interactable = false;
            _faltaEnvidoBtn.interactable = false;
        }
    }
    private void CambioTurno(int id) => SetInteractableButtonSate(_jugador.GetInstanceID() != id);
    private void OnWaitingStateChange(bool currentState) => SetInteractableButtonSate(currentState);
    private void SetInteractableButtonSate(bool state)
    {
        _trucoBtn.interactable = state;
        _envidoBtn.interactable = state;
        _realEnvidoBtn.interactable = state;
        _faltaEnvidoBtn.interactable = state;
    }
}
