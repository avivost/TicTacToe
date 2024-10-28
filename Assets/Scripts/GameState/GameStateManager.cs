using Board.BoardState;
using Board.Controller.Abstract;
using Board.Controller.Implementation;
using Board.Model;
using Board.Requests;
using Board.View.Abstract;
using GameOver.Controller.Abstract;
using GameOver.Controller.Implementation;
using GameOver.Requests;
using GameOver.View.Abstract;
using GameState;
using Unity.Mathematics;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
   public static GameStateManager Instance { get; private set; }
   
   [SerializeField] private int _lengh = 3;
   [SerializeField] private int _width = 3;
   [SerializeField] private GameObject _boardView;
   [SerializeField] private GameObject _gameOverView;
   
   public MarkerType CurrentMarkerType {get; private set;}
   private IBoardController _boardController;
   private IGameOverController _gameOverController;
   private GameStateType _currentState;
   private BaseBoardView _boardViewScript;
   private BaseGameOverView _gameOverViewScript;
   
   private void Awake()
   {
      // there is no use to call init twice
      if (Instance == this)
      {
         return;
      }
      
      // we have yet to initialize the instance, so we do it
      if (Instance == null)
      {
         Initialize();
      }
      
      // if we got here it means that we have already initialized the instance, and this instance is not the one we created before.
      // to ensure we truly have only one instance, we destroy the new one
      else
      {
         Destroy(this);
      }
   }
   
   public void ChangeState( BaseRequest baseRequest)
   {
      
      
      switch (baseRequest)
      {
         case EndTurnRequest turnEndedEvent:
         {
            HandleTurnEnded();
            break;
         }
         case StartNewGameRequest newGameRequestedEvent:
         {
            StartNewGame();
            return;
         }
      }
   }
   
   private void Initialize()
   {
      Instance = this;
      
      _gameOverController = new GameOverController();
      
      _boardView = Instantiate(_boardView, Vector3.zero, quaternion.identity);
      _boardView.SetActive(false);
      _boardViewScript = _boardView.GetComponent<BaseBoardView>();
      
      _gameOverView = Instantiate(_gameOverView, Vector3.zero, quaternion.identity);
      _gameOverView.SetActive(false);
      
      _gameOverViewScript = _gameOverView.GetComponent<BaseGameOverView>();
      _gameOverViewScript.Initialize(_gameOverController);
      StartNewGame();
   }
   
   private void HandleTurnEnded()
   {
      MarkerType? winner = _boardController.TryGetWinner();
      if (winner == null && !_boardController.IsBoardFull())
      {
         if (CurrentMarkerType == MarkerType.X)
            CurrentMarkerType = MarkerType.O;
         else
            CurrentMarkerType = MarkerType.X;
         _boardViewScript.Draw(_boardController.GetGrid());
         return;
      }
      
      _currentState = GameStateType.GameOver;
      _gameOverViewScript.Draw(winner);
   }
   
   private void StartNewGame()
   {
      BoardModel model = new BoardModel(_lengh, _width);
      _boardController = new BoardController(model);
      _boardViewScript.Initialize(_boardController);
      
      _currentState = GameStateType.Active;
      CurrentMarkerType = MarkerType.X;
      
      _gameOverView.SetActive(false);
      _boardView.SetActive(true);
      _boardViewScript.Draw(_boardController.GetGrid());
   }
   
   
}
