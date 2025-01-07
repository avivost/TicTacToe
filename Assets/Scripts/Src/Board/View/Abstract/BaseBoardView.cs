using Board.BoardState;
using Board.Controller.Abstract;
using Board.Model;
using Src.Board.Events;
using UnityEngine;

namespace Board.View.Abstract
{
    public interface IBoardViewSubject
    {
        delegate void MarkPlacementClicked(MarkPlacementClickedEvent markPlacedEvent);
        void RegisterOnMarkPlacementClicked( MarkPlacementClicked handler);
        void UnregisterOnMarkPlacementClicked( MarkPlacementClicked handler);
    }
    public abstract class BaseBoardView : MonoBehaviour, IBoardViewSubject
    {
        private event IBoardViewSubject.MarkPlacementClicked OnMarkPlacementClicked;
        public void Draw(MarkerType?[,] grid)
        {
         
            DrawBoardInternal(grid);
        }
        
        public void RegisterOnMarkPlacementClicked(IBoardViewSubject.MarkPlacementClicked handler)
        {
            OnMarkPlacementClicked += handler;
        }

        public void UnregisterOnMarkPlacementClicked(IBoardViewSubject.MarkPlacementClicked handler)
        {
            OnMarkPlacementClicked -= handler;
        }
        
        protected  void GetPlayerInput()
        {
            Vector2Int? playerInput = TryGetPlayerInputInternal();
            if (playerInput != null)
            {
                MarkPlacementClickedEvent markPlacedEvent = new MarkPlacementClickedEvent(playerInput.Value);
                OnMarkPlacementClicked?.Invoke(markPlacedEvent);
            }
            else
            {
                DrawInvalidInput();
            }
        }
        
        protected abstract Vector2Int? TryGetPlayerInputInternal();
        protected abstract void DrawBoardInternal(MarkerType?[,] model);
        protected abstract void DrawInvalidInput();
    }
}