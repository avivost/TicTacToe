using System.Collections.Generic;
using System.Linq;
using Board.BoardState;
using Board.Controller.Abstract;
using Board.Model;
using Board.Requests;
using UnityEngine;

namespace Board.Controller.Implementation
{
   
    class BoardController : IBoardController
    {
        private readonly BoardModel _model;
        
        public BoardController(BoardModel model)
        {
            _model = model;
        }
        
        public bool IsPlaceableInCell(Vector2Int cellPosition)
        {
            int x = cellPosition.x;
            int y = cellPosition.y;
            Vector2Int dimensions = _model.GetDimensions();
            int length = dimensions.x;
            int width = dimensions.y;
            
            if (x < 0 || x >= length || y < 0 || y >= width)
            {
                Debug.Log($"Invalid cell coordinates, x: {x}, y: {y}, length: {length}, width: {width}");
                return false;
            }
            
            return _model.GetCellState(cellPosition) == null;
        }

        public bool TryPlaceInCell(Vector2Int cellPosition)
        {
            if (!IsPlaceableInCell(cellPosition))
            {
                return false;
            }
            
            _model.SetCellState(cellPosition, GameStateManager.Instance.CurrentMarkerType);
            GameStateManager.Instance.ChangeState(new EndTurnRequest());
            return true;
        }

        public bool IsBoardFull()
        {
            Vector2Int dimensions = _model.GetDimensions();
            int length = dimensions.x;
            int width = dimensions.y;
            
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (_model.GetCellState(new Vector2Int(i, j)) == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public MarkerType? TryGetWinner()
        {
            Vector2Int dimensions = _model.GetDimensions();
            int length = dimensions.x;
            int width = dimensions.y;
            
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    MarkerType? winner = TryGetWinnerFromCell(new Vector2Int(i, j));
                    if (winner != null)
                    {
                        return winner;
                    }
                }
            }

            return null;
        }

        public MarkerType?[,] GetGrid()
        {
            Vector2Int dimensions = _model.GetDimensions();
            int length = dimensions.x;
            int width = dimensions.y;
            
            MarkerType?[,] grid = new MarkerType?[length, width];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    grid[i, j] = _model.GetCellState(new Vector2Int(i, j));
                }
            }

            return grid;
        }

        private MarkerType? TryGetWinnerFromCell(Vector2Int cellPosition)
        {
            MarkerType? state = _model.GetCellState(cellPosition);
            if (state == null)
            {
                return null;
            }

            Vector2Int dimensions = _model.GetDimensions();
            int length = dimensions.x;
            int width = dimensions.y;

            int x = cellPosition.x;
            int y = cellPosition.y;


            List<Vector2Int> horizontalPositions = GetPositions(amountOfCells:length, xStart:0, xIncrement:1, yStart:y, yIncrement:0);
            if (IsWinningLine(state, horizontalPositions))
            {
                return state;
            }
            
            List<Vector2Int> verticalPositions = GetPositions(amountOfCells:width, xStart:x, xIncrement:0, yStart:0, yIncrement:1);

            if (IsWinningLine(state, verticalPositions))
            {
                return state;
            }
            
            List<Vector2Int> diagonalTopLeftBottomRight = GetPositions(amountOfCells:length, xStart:0, xIncrement:1, yStart:0, yIncrement:1);

            if (x == y && IsWinningLine(state, diagonalTopLeftBottomRight))
            {
                return state;
            }
            
            List<Vector2Int> diagonalTopRightBottomLeft = GetPositions(amountOfCells:length, xStart:0, xIncrement:1, yStart:length -1 , yIncrement: -1);
            
            if (x + y == length - 1 && IsWinningLine(state, diagonalTopRightBottomLeft))
            {
                return state;
            }

            return null;
        }
        
        List<Vector2Int> GetPositions(int amountOfCells, int xStart, int xIncrement, int yStart, int yIncrement)
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            for (int i = 0; i < amountOfCells; i++)
            {
                positions.Add(new Vector2Int(xStart + i * xIncrement, yStart + i * yIncrement));
            }
            return positions;
        }
        
        private bool IsWinningLine(MarkerType? state, List<Vector2Int> positions)
        {
            foreach (var pos in positions)
            {
                if (_model.GetCellState(pos) != state)
                {
                    return false;
                }
            }

            return true;
        }
    }
}