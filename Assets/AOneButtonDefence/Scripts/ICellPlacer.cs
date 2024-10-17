using System;
using UnityEngine;

public interface ICellPlacer
{
    Action<CellPlacePosition> CellFilled { get; set; }
}