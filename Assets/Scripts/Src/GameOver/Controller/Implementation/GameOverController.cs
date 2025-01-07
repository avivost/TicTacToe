using Board.BoardState;
using GameOver.Controller.Abstract;
using GameOver.Requests;

namespace GameOver.Controller.Implementation
{
    public class GameOverController : IGameOverController
    {
        public void RestartGame()
        {
            GameStateManager.Instance.ChangeState(new StartNewGameRequest());
        }
    }
}