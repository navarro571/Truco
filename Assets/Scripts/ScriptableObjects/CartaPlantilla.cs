using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTipoCarta
{
    Basto,
    Espadilla,
    Oro,
    Copa
}

[CreateAssetMenu(menuName = "Carta", fileName = "Carta")]
public class CartaPlantilla : ScriptableObject
{
    public Sprite Imagen;
    public eTipoCarta Tipo;
    public int Numero;
    public bool Mata;
    public int Valor;
    public bool PosiblePieza;
    public int ValorPieza;
    public int ValorEnvido;
    public int ValorEnvidoPieza;
}
