using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelDePuntos : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI PuntosEquipoA;
    [SerializeField]
    private TextMeshProUGUI PuntosEquipoB;
    private void OnEnable()
    {
        GameController.FinRonda += UpdatePuntos;
    }

    private void OnDisable()
    {
        GameController.FinRonda -= UpdatePuntos;
    }

    private void UpdatePuntos(int PuntosEquipoA, int PuntosEquipoB)
    {
        this.PuntosEquipoA.text = PuntosEquipoA.ToString();
        this.PuntosEquipoB.text = PuntosEquipoB.ToString();
    }
}
