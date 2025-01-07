using Board.BoardState;
using GameOver.View.Abstract;
using GameState;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace GameOver.View.Implementation
{
    public class UIGameOverView  : BaseGameOverView
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _gameOverButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(OnGameOverClicked);
            _gameOverButton.onClick.AddListener(OnRestartClicked);
        }
        
        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnGameOverClicked);
            _gameOverButton.onClick.RemoveListener(OnRestartClicked);
        }

        protected override void DrawInternal(MarkerType? winner)
        {
            ViewManager.GetInstance().LoadView(gameObject);
        }

        private void OnGameOverClicked()
        {
            InvokeOnGameOverClicked();
        }
        
        private void OnRestartClicked()
        {
            InvokeOnRestartClicked();
        }
    }
}