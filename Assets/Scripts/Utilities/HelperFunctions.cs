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
        public static int Contar(GameObject cartasObj)
        {
            Carta[] cartas = cartasObj.GetComponentsInChildren<Carta>();
            int monto = 0;
            if (cartas != null)
            {
                InfoCartas infoCartas = ObtenerPiezas(cartas);
                if (infoCartas.flor)
                {
                    Carta[] piezasOrdenadas = (from c in infoCartas.piezas orderby c.ValorEnvido descending select c).ToArray();
                    Carta[] cartasNoPieza = Array.FindAll(cartas, carta => !carta.Pieza);
                    int length = piezasOrdenadas.Length > cartasNoPieza.Length ? piezasOrdenadas.Length : cartasNoPieza.Length;
                    for (int i = 0; i < length; i++)
                    {
                        Carta pieza = piezasOrdenadas.ElementAtOrDefault(i);
                        Carta carta = cartasNoPieza.ElementAtOrDefault(i);
                        if(pieza != null)
                        {
                            if (i == 0)
                            {
                                monto += pieza.ValorEnvido;
                            }
                            else
                            {
                                string numberString = pieza.ValorEnvido.ToString();
                                monto += Int32.Parse(numberString[numberString.Length - 1].ToString());
                            }

                        }
                        if (carta != null)
                            monto += carta.ValorEnvido;
                    }
                }
                else
                {
                    Carta[] mismoPalo = ObtenerCartasMismoPalo(cartas);
                    Carta pieza = infoCartas.piezas.Length > 0 ? infoCartas.piezas[0] : null;
                    Carta[] cartasOrdenadas = (from c in cartas where !c.Pieza orderby c.ValorEnvido descending select c).ToArray();
                    if (pieza != null)
                    {
                        monto = pieza.ValorEnvido + cartasOrdenadas[0].ValorEnvido;
                    }
                    else if (mismoPalo.Length > 0)
                    {
                        int montoMismoPalo = 20;
                        monto = montoMismoPalo + mismoPalo[0].ValorEnvido + mismoPalo[1].ValorEnvido;
                    }
                    else
                    {
                        monto = cartasOrdenadas[0].ValorEnvido;
                    }
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
