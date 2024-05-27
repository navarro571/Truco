using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carta : MonoBehaviour
{
    [SerializeField]
    private CartaPlantilla plantilla;
    public CartaPlantilla CartaPlantilla
    {
        get { return plantilla; }
        set
        {
            GetComponent<SpriteRenderer>().sprite = value.Imagen;
            plantilla = value;
        }
    }
    private bool tapada;

    public bool Tapada
    {
        get { return tapada; }
        set
        {
            if (value) 
                GetComponent<SpriteRenderer>().sprite = SpriteTapada;
            else
                GetComponent<SpriteRenderer>().sprite = CartaPlantilla.Imagen;
            tapada = value;
        }
    }
    public int Valor;
    public int ValorEnvido;
    public bool Pieza;
    public bool esMuestra;
    public bool isDraggable;
    public bool IACarta;
    public bool CartaJugada;
    public Sprite SpriteTapada;

    private Vector3 screenPoint;
    private Vector3 offset;
    private Quaternion prevRot;
    private IJugador jugadorPadre;
    private GameController gameController;

    private void Awake()
    {
        jugadorPadre = GetComponentInParent<IJugador>();
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        isDraggable = !esMuestra && !IACarta && jugadorPadre.MiTurno;
    }

    private void OnEnable()
    {
        ActionController.OnWaitingStateChange += OnWaitingStateChange;
    }
    private void OnDisable()
    {
        ActionController.OnWaitingStateChange -= OnWaitingStateChange;
    }

    void OnMouseDown()
    {
        if (isDraggable)
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            prevRot = gameObject.transform.rotation;
        }
    }

    void OnMouseDrag()
    {
        if (isDraggable)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    void OnMouseUp()
    {
        if (isDraggable)
        {
            transform.position = jugadorPadre.CartaPosicionFinal.position;
            CartaJugada = true;
            jugadorPadre.CartaJugada(this);
        }
    }

    private void OnWaitingStateChange(bool currentState)
    {
        isDraggable = !currentState;
    }
}
