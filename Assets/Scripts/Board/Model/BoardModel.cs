using Board.BoardState;
using UnityEngine;

namespace Board.Model
{
    public class BoardModel
    {
        private MarkerType?[,] _grid;

        public BoardModel(int length, int width)
        {
            _grid = new MarkerType?[length, width];
        }
        
        public void SetCellState(Vector2Int cellPosition, MarkerType state)
        {
               int x = cellPosition.x;
               int y = cellPosition.y;
               _grid[x, y] = state;
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
        
       
    }
}