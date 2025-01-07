using UnityEngine;

namespace Src.Board.Events
{
    public class MarkPlacementClickedEvent
    {
        public readonly Vector2Int Position;

        public MarkPlacementClickedEvent(Vector2Int position)
        {
            Position = position;
        }
    }
}