using System;
using Board.BoardState;
using GameState;
using UnityEngine;

namespace GameOver.View.Abstract
{
    public interface IGameOverViewSubject
    {
        
        void RegisterOnGameOverClicked(Action handler);
        void UnregisterOnGameOverClicked(Action handler);
        
        void RegisterOnRestartClicked(Action handler);
        void UnregisterOnRestartClicked(Action handler);
    }
    public abstract class BaseGameOverView : MonoBehaviour, IGameOverViewSubject
    {

        protected MarkerType? Winner { get; private set; }
        
        private event Action OnGameOverClicked;
        private event Action OnRestartClicked;
        
        
        public void Draw(MarkerType? winner)
        {
            ViewManager.GetInstance().LoadView(gameObject);
            DrawInternal(winner);
        }
        
        public void RegisterOnGameOverClicked(Action handler)
        {
            OnGameOverClicked += handler;
        }

        public void UnregisterOnGameOverClicked(Action handler)
        {
            OnGameOverClicked -= handler;
        }

        public void RegisterOnRestartClicked(Action handler)
        {
            OnRestartClicked += handler;
        }

        public void UnregisterOnRestartClicked(Action handler)
        {
            OnRestartClicked -= handler;
        }
        
        protected abstract void DrawInternal(MarkerType? winner);
        
        protected void InvokeOnGameOverClicked()
        {
            OnGameOverClicked?.Invoke();
        }
        
        protected void InvokeOnRestartClicked()
        {
            OnRestartClicked?.Invoke();
        }
    }
}