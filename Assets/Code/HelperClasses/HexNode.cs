using Grid;
using Priority_Queue;

public class HexNode : PriorityQueueNode
{
    public HexNode(Hex hex)
    {
        ContainedHex = hex;
    }

    public Hex ContainedHex { get; private set; }
}
