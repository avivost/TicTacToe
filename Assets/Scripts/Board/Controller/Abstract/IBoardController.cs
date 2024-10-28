using Board.BoardState;
using UnityEngine;

namespace Board.Controller.Abstract
{
    public interface IBoardController
    {
        bool IsPlaceableInCell(Vector2Int cellPosition);
        bool TryPlaceInCell(Vector2Int cellPosition);
        
        bool IsBoardFull();
        MarkerType? TryGetWinner();
        
        MarkerType?[,] GetGrid();
    }
}