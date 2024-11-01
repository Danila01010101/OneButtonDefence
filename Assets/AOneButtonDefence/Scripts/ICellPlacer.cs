using System;
using UnityEngine;

public interface ICellPlacer
{
    Action<CellPlaceCoordinates> CellFilled { get; set; }
}