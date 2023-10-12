using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTipoAccion
{
    SINRESPUESTA,
    RESPUESTA
}

public enum eEstrategia
{
    florCanto,
    flor,
    florEnvido,
    florResto,
    Envido,
    RealEnvido,
    FaltaEnvido,
    Truco,
    Retruco,
    ValeCuatro
}

public class GameController : MonoBehaviour
{
    public static event Action<int> CambioTurno;
    public static event Action<int, int> FinRonda;

    [Header("SETTINGS")]
    public float distanciaCartas;
    public CartaPlantilla[] ListaCartas;

    [Header("PREFABS")]
    public GameObject CartasPrefab;
    public GameObject CartaPrefab;
    public GameObject EquipoPrefab;
    public GameObject JugadorPrefab;
    public GameObject BotPrefab;

    [Header("POSITIONS")]
    public Transform PositionPlayer;
    public Transform PositionIA;
    public Transform CartaPosicionBot;
    public Transform CartaPosicionJugador;
    public Transform CartaPosicionMuestraDer;
    public Transform CartaPosicionMuestraIzq;

    [Header("TEST FUNC")]
    public bool ForzarCartas;
    public CartaPlantilla MuestraForzada;
    public CartaPlantilla[] CartasForzadas;

    [Header("INFO")]
    public int ManoActualPuntosEquipoA;
    public int ManoActualPuntosEquipoB;
    public int PuntosEquipoA;
    public int PuntosEquipoB;

    [Header("JUGADOR")]
    [SerializeField]
    private GameObject JugadorObj;
    private GameObject cartasJugador;
    [Header("IA")]
    [SerializeField]
    private GameObject IAObj;
    private GameObject cartasIA;
    [SerializeField]
    private Carta CartaLanzadaJugador;
    private Carta CartaLanzadaIA;
    [SerializeField]
    private List<Carta> CartasJugadas = new List<Carta>();
    [SerializeField]
    private Carta Muestra;
    [SerializeField]
    private int trucoNivel;
    [SerializeField]
    private int envidoPuntos;

    private Transform muestraPosition;
    private ProvedorCarta provedor;
    private GameObject[] Equipos;
    private GameObject turno;

    private void Awake()
    {
        provedor = GetComponent<ProvedorCarta>();
    }

    void Start()
    {
        PuntosEquipoA = 0;
        PuntosEquipoB = 0;
        ManoActualPuntosEquipoA = 0;
        ManoActualPuntosEquipoB = 0;
        muestraPosition = Math.Sign(UnityEngine.Random.Range(-10, 10)) > 0 ? CartaPosicionMuestraIzq : CartaPosicionMuestraDer;

        GameObject muestra = Instantiate(CartaPrefab, muestraPosition.position, Quaternion.identity, null);
        Carta MuestraCarta = muestra.GetComponent<Carta>();
        MuestraCarta.CartaPlantilla = ForzarCartas ? MuestraForzada : provedor.ObtenerCarta();
        MuestraCarta.esMuestra = true;
        Muestra = MuestraCarta;

        GameObject team1 = InicializarJugador();
        GameObject team2 = InicializarIA();

        Equipos = new GameObject[] { team1, team2 };

        if (muestraPosition == CartaPosicionMuestraDer)
        {
            turno = IAObj;
        } 
        else
        {
            turno = JugadorObj;
        }

        CambioTurno?.Invoke(turno.GetInstanceID());
    }

    private void OnEnable()
    {
        Jugador.PlayerAction += PlayerAction;
        IA.IAAction += IAAction;
    }

    private void OnDisable()
    {
        Jugador.PlayerAction -= PlayerAction;
        IA.IAAction -= IAAction;
    }

    public Carta ObtenerMuestra() => Muestra;

    private GameObject InicializarJugador()
    {
        GameObject team1 = Instantiate(EquipoPrefab);
        Equipo team1Component = team1.GetComponent<Equipo>();
        GameObject jugadorObj = Instantiate(JugadorPrefab, team1.transform);
        Jugador jugador = jugadorObj.GetComponent<Jugador>();
        Transform area = jugadorObj.transform.Find("Area");
        Transform camera = jugadorObj.transform.Find("PlayerCamera");

        area.position = PositionPlayer.position;
        area.localRotation = PositionPlayer.rotation;
        camera.localRotation = PositionPlayer.rotation;

        GameObject cartas = GenerarCartas(area);
        cartasJugador = cartas;
        jugador.DarCartas(cartas);

        this.JugadorObj = jugadorObj;
        team1Component.Jugadores = new GameObject[] { jugadorObj };

        return team1;
    }

    private GameObject InicializarIA()
    {
        GameObject team2 = Instantiate(EquipoPrefab);
        Equipo team2Component = team2.GetComponent<Equipo>();
        GameObject IAObj = Instantiate(BotPrefab, team2.transform);
        IA IA = IAObj.GetComponent<IA>();
        Transform area = IA.transform.Find("Area");

        area.position = PositionIA.position;
        area.localRotation = PositionIA.rotation;

        GameObject cartas = GenerarCartas(area, true);
        cartasIA = cartas;
        IA.DarCartas(cartas);

        this.IAObj = IAObj;
        team2Component.Jugadores = new GameObject[] { IAObj };

        return team2;
    }

    private void IAAction (eTipoAccion tipoAccion, object Data)
    {
        if(tipoAccion == eTipoAccion.SINRESPUESTA)
        {
            CartaLanzadaIA = (Carta)Data;
            CartasJugadas.Add((Carta)Data);
            StartCoroutine(CartaJugada());
        } 
        else
        {
            eEstrategia estrategia = (eEstrategia)Data;

            switch (estrategia)
            {
                case eEstrategia.florCanto:
                    PuntosEquipoB += 3;
                    break;
                case eEstrategia.flor:
                    break;
                case eEstrategia.florEnvido:
                    break;
                case eEstrategia.florResto:
                    break;
                case eEstrategia.Envido:
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
        }
    }

    private void PlayerAction(eTipoAccion tipoAccion, object Data)
    {
        if (tipoAccion == eTipoAccion.SINRESPUESTA)
        {
            CartaLanzadaJugador = (Carta)Data;
            CartasJugadas.Add((Carta)Data);
            StartCoroutine(CartaJugada());
        }
        else
        {
            eEstrategia estrategia = (eEstrategia)Data;

            switch (estrategia)
            {
                case eEstrategia.florCanto:
                    PuntosEquipoA += 3;
                    break;
                case eEstrategia.flor:
                    break;
                case eEstrategia.florEnvido:
                    break;
                case eEstrategia.florResto:
                    break;
                case eEstrategia.Envido:
                    HelperFunctions.CalcularEnvido(cartasJugador, Muestra);
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
        }
    }

    private IEnumerator CartaJugada()
    {

        if (CartaLanzadaJugador != null && CartaLanzadaIA != null)
        {
            bool rondaTerminada = false;
            int CartaJugadorValor = CartaLanzadaJugador.Valor;
            int CartaIAValor = CartaLanzadaIA.Valor;

            if (CartaJugadorValor > CartaIAValor)
            {
                ++ManoActualPuntosEquipoA;
                turno = JugadorObj;
            }
            else if (CartaIAValor > CartaJugadorValor)
            {
                ++ManoActualPuntosEquipoB;
                turno = IAObj;
            }
            else
            {
                ++ManoActualPuntosEquipoA;
                ++ManoActualPuntosEquipoB;
            }

            if (ManoActualPuntosEquipoA > 3 || ManoActualPuntosEquipoA == 2 && ManoActualPuntosEquipoB <= 1)
            {
                ++PuntosEquipoA;
                rondaTerminada = true;
            }
            else if (ManoActualPuntosEquipoB > 3 || ManoActualPuntosEquipoB == 2 && ManoActualPuntosEquipoA <= 1)
            {
                ++PuntosEquipoB;
                rondaTerminada = true;
            }

            yield return new WaitForSeconds(3f);

            if (!rondaTerminada)
            {
                CartaLanzadaJugador?.gameObject.SetActive(false);
                CartaLanzadaIA?.gameObject.SetActive(false);
                CartaLanzadaJugador = null;
                CartaLanzadaIA = null;
                CambioTurno?.Invoke(turno != null ? turno.GetInstanceID() : -1);
            }
            else
                RondaTerminada();
        }
        else
        {
            if (turno != null)
                CambioTurno?.Invoke(turno.GetInstanceID() == JugadorObj.GetInstanceID() ? IAObj.GetInstanceID() : JugadorObj.GetInstanceID());
        }
    }

    public void RondaTerminada()
    {
        ManoActualPuntosEquipoA = 0;
        ManoActualPuntosEquipoB = 0;
        CartaLanzadaJugador = null;
        CartaLanzadaIA = null;
        provedor.Reset();
        CambioTurno?.Invoke(-1);
        FinRonda?.Invoke(PuntosEquipoA, PuntosEquipoB);
        //reiniciar ronda

        if(muestraPosition == CartaPosicionMuestraIzq)
        {
            muestraPosition = CartaPosicionMuestraDer;
            turno = IAObj;
        } else
        {
            muestraPosition = CartaPosicionMuestraIzq;
            turno = JugadorObj;
        }

        Muestra.gameObject.transform.position = muestraPosition.position;
        Muestra.CartaPlantilla = provedor.ObtenerCarta();

        Jugador jugador = JugadorObj.GetComponent<Jugador>();
        IA ia = IAObj.GetComponent<IA>();
        Transform areaJugador = JugadorObj.transform.Find("Area");
        Transform areaIA = ia.transform.Find("Area");

        Destroy(jugador.cartasGameObject);
        jugador.cartasGameObject = null;
        Destroy(ia.cartasGameObject);
        ia.cartasGameObject = null;

        cartasJugador = GenerarCartas(areaJugador);
        jugador.DarCartas(cartasJugador);
        cartasIA = GenerarCartas(areaIA, true);
        ia.DarCartas(cartasIA);

        CambioTurno?.Invoke(turno.GetInstanceID());
    }

    private GameObject GenerarCartas(Transform Area, bool CartasIA = false)
    {
        GameObject cartas = Instantiate(CartasPrefab, Area);
        Carta[] componentesCartas = cartas.GetComponentsInChildren<Carta>();
        for (int i = 0; i < componentesCartas.Length; i++)
        {
            Carta carta = componentesCartas[i];
            carta.CartaPlantilla = ForzarCartas ? CartasForzadas[i] : provedor.ObtenerCarta();
            carta.Pieza = carta.CartaPlantilla.PosiblePieza &&
                carta.CartaPlantilla.Tipo == Muestra.CartaPlantilla.Tipo &&
                carta.CartaPlantilla.Numero != 12;

            carta.Valor = carta.Pieza ? carta.CartaPlantilla.ValorPieza : carta.CartaPlantilla.Valor;
             if (carta.CartaPlantilla.Numero == 12 &&
                Muestra.CartaPlantilla.PosiblePieza &&
                carta.CartaPlantilla.Tipo == Muestra.CartaPlantilla.Tipo)
                carta.Valor = Muestra.CartaPlantilla.ValorPieza;

            if (CartasIA)
            {
                carta.IACarta = true;
                carta.Tapada = true;
            }
        }

        return cartas;
    }
}
