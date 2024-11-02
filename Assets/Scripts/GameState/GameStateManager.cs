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
using UnityEngine.Serialization;

public class GameStateManager : MonoBehaviour
{
   public static GameStateManager Instance { get; private set; }
   
   [SerializeField] private int _length = 3;
   [SerializeField] private int _width = 3;
   [SerializeField] private BaseBoardView _boardViewPrefab;
   [SerializeField] private BaseGameOverView _gameOverViewPrefab;
   
   public MarkerType CurrentMarkerType {get; private set;}
   private IBoardController _boardController;
   private IGameOverController _gameOverController;
   private GameStateType _currentState;
   private BaseBoardView _boardViewScript;
   private BaseGameOverView _gameOverViewScript;
   
   private void Awake()
   {
      // if there is no instance, we initialize it
      if (Instance == null)
      {
         Initialize();
      }
      // if this instance is not the original, we destroy it
      else if (Instance != this)
      {
         Destroy(gameObject);
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
      
      _boardViewScript = Instantiate(_boardViewPrefab, Vector3.zero, quaternion.identity);
      _boardViewScript.gameObject.SetActive(false);
      
      _gameOverViewScript = Instantiate(_gameOverViewPrefab, Vector3.zero, quaternion.identity);
      _gameOverViewPrefab.gameObject.SetActive(false);
      
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
      BoardModel model = new BoardModel(_length, _width);
      _boardController = new BoardController(model);
      _boardViewScript.Initialize(_boardController);
      
      _currentState = GameStateType.Active;
      CurrentMarkerType = MarkerType.X;
      
      _gameOverViewScript.gameObject.SetActive(false);
      _boardViewScript.gameObject.SetActive(true);
      _boardViewScript.Draw(_boardController.GetGrid());
   }
   
   
}
