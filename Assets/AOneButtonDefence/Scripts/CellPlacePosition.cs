public struct CellPlacePosition
{
    public int X { get; private set; }
    public int Z { get; private set; }

    public CellPlacePosition(int xPosition, int yPosition)
    {
        X = xPosition;
        Z = yPosition;
    }
}