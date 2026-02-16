public enum Lane
{
    Left = -1,
    Center = 0,
    Right = 1
}

public enum SegmentType
{
    Safe,
    Single,
    Double,
    Shift,
    Recovery
}

public class WeightedItem
{
    public SegmentType segmentType;
    public int weight;
}