using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    interface IGameMode
    {
        Vector3[] PlayerPositions { get; set; }
        Vector3[] PlayerRotations { get; set; }

        void Initialize();
        void GameStart();
    }
}
