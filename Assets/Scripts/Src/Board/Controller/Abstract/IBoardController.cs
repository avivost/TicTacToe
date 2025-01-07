using System;
using Board.BoardState;
using UnityEngine;

namespace Board.Controller.Abstract
{
    public interface IBoardController
    {
        bool IsBoardFull();
        MarkerType? TryGetWinner();
        
        MarkerType?[,] GetGrid();
    }
}