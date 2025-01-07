using Board.BoardState;
using Src.Board.Events;
using UnityEngine;

namespace Board.Model
{
    public class BoardModel : IBoardSubject
    {
        private MarkerType?[,] _grid;
        
        private event IBoardSubject.GridChangeHandler GridChangeHandler;

        public BoardModel(int length, int width)
        {
            _grid = new MarkerType?[length, width];
        }
        
        public void SetCellState(Vector2Int cellPosition, MarkerType state)
        {
               int x = cellPosition.x;
               int y = cellPosition.y;
               _grid[x, y] = state;
               GridChangeHandler?.Invoke(new MarkPlacedEvent(cellPosition, _grid));
        }
        
        public MarkerType? GetCellState(Vector2Int cellPosition)
        {
            return _grid[cellPosition.x, cellPosition.y];
        }
        
        public Vector2Int GetDimensions()
        {
            return new Vector2Int(GetLength(), GetWidth());
        }
        
        public int GetLength()
        {
            return _grid.GetLength(0);
        }
        
        public int GetWidth()
        {
            return _grid.GetLength(1);
        }


        public void RegisterGridChange(IBoardSubject.GridChangeHandler handler)
        {
            GridChangeHandler += handler;
        }

        public void UnregisterGridChange(IBoardSubject.GridChangeHandler handler)
        {
            GridChangeHandler -= handler;
        }
    }
}