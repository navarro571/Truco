using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    class InfoCartas {
        public Carta[] piezas { get; set; }
        public bool flor { get; set; }
    }
    class HelperFunctions
    {
        public static int CalcularEnvido(GameObject cartasObj, Carta muestra)
        {
            Carta[] cartas = cartasObj.GetComponentsInChildren<Carta>();
            int monto = 0;
            if (cartas != null)
            {
                Carta[] mismoPalo = ObtenerCartasMismoPalo(cartas);
                Carta pieza = Array.Find(cartas, carta => carta.Pieza);
                Carta[] cartasOrdenadas = (from c in cartas where !c.Pieza orderby c.CartaPlantilla.ValorEnvido descending select c).ToArray();
                if (pieza != null)
                {
                    monto = pieza.CartaPlantilla.ValorEnvidoPieza + cartasOrdenadas[0].CartaPlantilla.ValorEnvido;
                } 
                else if(mismoPalo.Length > 0)
                {
                    int montoMismoPalo = 20;
                    monto = montoMismoPalo + mismoPalo[0].CartaPlantilla.ValorEnvido + mismoPalo[1].CartaPlantilla.ValorEnvido;
                } else
                {
                    monto = cartasOrdenadas[0].CartaPlantilla.ValorEnvido;
                }
            }
            return monto;
        }

        public static InfoCartas ObtenerPiezas(Carta[] cartas)
        {
            Carta[] piezas = Array.FindAll(cartas, (carta) => carta.Pieza);
            bool flor = false;
            int mismoPalo = ObtenerCartasMismoPalo(cartas).Length;

            if (mismoPalo == 3 || piezas.Length >= 2 || piezas.Length == 1 && mismoPalo == 2)
                flor = true;

            return new InfoCartas()
            {
                flor = flor,
                piezas = piezas
            };
        }

        public static Carta[] ObtenerCartasMismoPalo(Carta[] cartas)
        {
            Carta[] mismoPalo = Array.Empty<Carta>();
            Dictionary<eTipoCarta, int> infoCartas = new();
            foreach (Carta carta in cartas)
            {
                if (carta.Pieza) continue;
                eTipoCarta tipoActual = carta.CartaPlantilla.Tipo;
                if (infoCartas.ContainsKey(tipoActual))
                {
                    infoCartas.TryGetValue(tipoActual, out int value);
                    infoCartas[tipoActual] = ++value;
                }
                else
                {
                    infoCartas.Add(tipoActual, 1);
                }
            }
            foreach (KeyValuePair<eTipoCarta, int> carta in infoCartas)
            {
                if (carta.Value > 1)
                {
                    mismoPalo = (from c in cartas where c.CartaPlantilla.Tipo == carta.Key select c).ToArray();
                }
            }

            return mismoPalo;
        }
    }
}
