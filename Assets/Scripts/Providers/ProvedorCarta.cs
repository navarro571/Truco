using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvedorCarta : MonoBehaviour
{
    public CartaPlantilla[] ListaCartas;
    public List<CartaPlantilla> CartasUsadas = new List<CartaPlantilla>();

    public CartaPlantilla ObtenerCarta()
    {
        CartaPlantilla cartaSeleccionada;
        CartaPlantilla existente;
        do
        {
            cartaSeleccionada = ListaCartas[UnityEngine.Random.Range(0, ListaCartas.Length - 1)];
            existente = CartasUsadas.Find((carta) =>
            {
                return carta.Numero == cartaSeleccionada.Numero && carta.Tipo == cartaSeleccionada.Tipo;
            });
        } while (existente != null);
        CartasUsadas.Add(cartaSeleccionada);
        return cartaSeleccionada;
    }

    public void Reset()
    {
        CartasUsadas.Clear();
    }
}
