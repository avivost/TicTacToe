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

        private void Awake()
        {
            _restartButton.onClick.AddListener(OnClick);
        }
        
        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnClick);
        }

        protected override void DrawInternal(MarkerType? winner)
        {
            ViewManager.GetInstance().LoadView(gameObject);
        }

        private void OnClick()
        {
            GameOverController.RestartGame();
        }
    }
}