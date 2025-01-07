using Src.Board.Events;

namespace Board.Model
{
    public interface IBoardSubject
    {
        public delegate void GridChangeHandler(MarkPlacedEvent placedEvent);

        public void RegisterGridChange(GridChangeHandler handler);
        public void UnregisterGridChange(GridChangeHandler handler);
    }
}