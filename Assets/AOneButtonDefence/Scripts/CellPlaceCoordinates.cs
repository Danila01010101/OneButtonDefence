public struct CellPlaceCoordinates
{
    public int X { get; private set; }
    public int Z { get; private set; }

    public CellPlaceCoordinates(int xPosition, int yPosition)
    {
        X = xPosition;
        Z = yPosition;
    }
}