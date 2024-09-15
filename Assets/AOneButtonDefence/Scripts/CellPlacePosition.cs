public struct CellPlacePosition
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public CellPlacePosition(int xPosition, int yPosition)
    {
        X = xPosition;
        Y = yPosition;
    }
}