using Board.BoardState;
using GameOver.Controller.Abstract;
using GameState;
using UnityEngine;

namespace GameOver.View.Abstract
{
    public abstract class BaseGameOverView : MonoBehaviour
    {
        private bool _isInitialized;

        protected MarkerType? Winner { get; private set; }
        protected IGameOverController GameOverController;
        
        public void Initialize(IGameOverController gameOverController)
        {
            _isInitialized = true;
            GameOverController = gameOverController;
        }
        
        public void Draw(MarkerType? winner)
        {
            ViewManager.GetInstance().LoadView(gameObject);
            if (!_isInitialized)
            {
                throw new System.Exception("View is not initialized");
            }
            DrawInternal(winner);
        }
        
        protected abstract void DrawInternal(MarkerType? winner);
    }
}