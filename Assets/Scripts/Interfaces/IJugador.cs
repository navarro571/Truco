using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IJugador
    {
        bool MiTurno { get; set; }
        Transform CartaPosicionFinal { get; set; }
        void ValidarCartas();
        void CartaJugada(Carta carta);
    }
}
