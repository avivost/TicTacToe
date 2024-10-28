using Board.BoardState;
using Board.Controller.Abstract;
using Board.Model;
using UnityEngine;

namespace Board.View.Abstract
{
    public abstract class BaseBoardView : MonoBehaviour
    {
        public bool IsInitialized => _isInitialized;
        protected IBoardController BoardController { get; private set; }
        
        private bool _isInitialized = false;
        


        public void Initialize(IBoardController boardController)
        {
            _isInitialized = true;
            BoardController = boardController;
        }

        public void Draw(MarkerType?[,] grid)
        {
            if (!IsInitialized)
            {
                throw new System.Exception("View is not initialized");
            }
            DrawBoardInternal(grid);
        }

        protected abstract Vector2Int? TryGetPlayerInputInternal();
        protected abstract void DrawBoardInternal(MarkerType?[,] model);
        
        protected  void GetPlayerInput()
        {
            if (!IsInitialized)
            {
                throw new System.Exception("View is not initialized");
            }
            
            Vector2Int? playerInput = null;
            while (playerInput == null)
            {
                playerInput = TryGetPlayerInputInternal();
            }

            BoardController.TryPlaceInCell(playerInput.Value);
        }
    }
}