using System.Collections;
using System.Collections.Generic;
using Board.BoardState;
using Board.Controller.Implementation;
using Board.Model;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Board.Controller.Implementation;
using Board.View.Abstract;

public class BoardTest
{
    // mocked means that we are not testing the actual implementation of the BoardView,
    // but we are testing the BoardController, which uses the BoardView
    // we are using a mock object to simulate the BoardView
    // and satisfy the dependency of the BoardController
    private class MockBoardViewSubject : IBoardViewSubject
    {
        public void RegisterOnMarkPlacementClicked(IBoardViewSubject.MarkPlacementClicked handler)
        {
        }

        public void UnregisterOnMarkPlacementClicked(IBoardViewSubject.MarkPlacementClicked handler)
        {
        }
    }
  
    //flow_state_expected
    [Test]
    public void WinCondition_diagonalLineOfXs_XWins()
    {
        //setup
        BoardModel board = new BoardModel(3, 3);
        BoardController boardController = new BoardController(board, new MockBoardViewSubject());
        
        board.SetCellState(new Vector2Int(0,0),MarkerType.X);
        board.SetCellState(new Vector2Int(1,1),MarkerType.X);
        board.SetCellState(new Vector2Int(2,2),MarkerType.X);

        //act
        MarkerType? winnerType = boardController.TryGetWinner();

        //assert
        Assert.Equals(MarkerType.X, winnerType);
    }
    
    [Test]
    public void WinCondition_diagonalLineOfOs_OWins()
    {
        //setup
        BoardModel board = new BoardModel(3, 3);
        BoardController boardController = new BoardController(board, new MockBoardViewSubject());
        board.SetCellState(new Vector2Int(0,0),MarkerType.O);
        board.SetCellState(new Vector2Int(1,1),MarkerType.O);
        board.SetCellState(new Vector2Int(2,2),MarkerType.O);

        //act
        MarkerType? winnerType = boardController.TryGetWinner();

        //assert
        Assert.True(winnerType == MarkerType.O);
    }
    
}
