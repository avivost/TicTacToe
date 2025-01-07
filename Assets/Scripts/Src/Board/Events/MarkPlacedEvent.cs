using Board.BoardState;
using UnityEngine;

namespace Src.Board.Events
{
    public class MarkPlacedEvent
    {
        // the Position field is not used in the current implementation, in some situations we would prefer to have slim, atomic events.
        // in those cases the events will only contain the delta of the change and not the entire state.
        public readonly Vector2Int Position;
        
        public readonly MarkerType?[,] CurrentState;

        public MarkPlacedEvent(Vector2Int position, MarkerType?[,] currentState)
        {
            Position = position;
            CurrentState = currentState;
        }
    }
}