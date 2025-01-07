using System.Collections.Generic;
using System.Linq;
using Board.BoardState;
using Board.Controller.Abstract;
using Board.Model;
using Board.Requests;
using UnityEngine;

namespace Board.Controller.Implementation
{
   
    public class BoardController : IBoardController
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
            
            // Ensure cell coordinates are within the board's range
            if (x < 0 || x >= length || y < 0 || y >= width)
            {
                Debug.Log($"Invalid cell coordinates, x: {x}, y: {y}, length: {length}, width: {width}");
                return false;
            }
            
            // Check if the cell is empty.
            return _model.GetCellState(cellPosition) == null;
        }

        public bool TryPlaceInCell(Vector2Int cellPosition)
        {
            if (!IsPlaceableInCell(cellPosition))
            {
                return false;
            }
            // Place the marker
            _model.SetCellState(cellPosition, GameStateManager.Instance.CurrentMarkerType);
            
            // trigger the end-turn request.
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
                    // if even one cell contains a null aka it's empty, the board is not full.
                    // and we can return false.
                    if (_model.GetCellState(new Vector2Int(i, j)) == null)
                    {
                        return false;
                    }
                }
            }
            
            //if we got to this line of code, it means we didn't find any empty cells, so the board is full.
            return true;
        }
        
        
        /// <summary>
        /// Iterate through each cell on the board, attempting to find a winning line.
        /// This method checks each cell to determine if it starts a line of markers (horizontal, vertical, or diagonal)
        /// that meets the winning condition.
        /// </summary>
        public MarkerType? TryGetWinner()
        {
            Vector2Int dimensions = _model.GetDimensions();
            int length = dimensions.x;
            int width = dimensions.y;
            
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector2Int cellPosition = new Vector2Int(i, j);
                    // Skip empty cells.
                    if (_model.GetCellState(cellPosition) == null)
                    {
                        continue;
                    }
                    // Attempt to find a winner starting from the current cell.
                    MarkerType? winner = TryGetWinnerFromCell(cellPosition);
                    if (winner != null)
                    {
                        return winner; // Return the winner marker type if found.
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
        
        /// <summary>
        /// Check for any winning line (horizontal, vertical, or diagonal) starting from a given cell.
        /// This method evaluates all possible directions to see if a continuous line of identical markers is formed.
        /// </summary>
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

        /// <summary>
        ///  Generate a sequence of cell positions in a straight line based on start points and increments.
        ///  This is useful for constructing potential winning lines in horizontal, vertical, or diagonal directions.
        /// </summary>
        List<Vector2Int> GetPositions(int amountOfCells, int xStart, int xIncrement, int yStart, int yIncrement)
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            for (int i = 0; i < amountOfCells; i++)
            {
                positions.Add(new Vector2Int(xStart + i * xIncrement, yStart + i * yIncrement));
            }
            return positions;
        }
        
        
        /// <summary>
        ///  Verify if all cells in a given line have the same marker type, indicating a winning line.
        /// </summary>
        private bool IsWinningLine(MarkerType? state, List<Vector2Int> positions)
        {
            foreach (var pos in positions)
            {
                if (_model.GetCellState(pos) != state)
                {
                    return false; // Return false if any cell does not match the required state.
                }
            }

            return true; // Return true if all cells in the line have the same marker type.
        }
    }
}