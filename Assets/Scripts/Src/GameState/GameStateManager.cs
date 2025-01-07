using Board.BoardState;
using Board.Controller.Abstract;
using Board.Controller.Implementation;
using Board.Model;
using Board.View.Abstract;
using GameOver.View.Abstract;
using GameState;
using Src.Board.Events;
using Unity.Mathematics;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
   public static GameStateManager Instance { get; private set; }
   
   [SerializeField] private int _length = 3;
   [SerializeField] private int _width = 3;
   [SerializeField] private BaseBoardView _boardViewPrefab;
   [SerializeField] private BaseGameOverView _gameOverViewPrefab;
   
   public MarkerType CurrentMarkerType {get; private set;}
   private IBoardController _boardController;
   private GameStateType _currentState;
   private BaseBoardView _boardView;
   private BaseGameOverView _gameOverView;
   
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
   
   private void Initialize()
   {
      Instance = this;
      QualitySettings.vSyncCount = 0;
      Application.targetFrameRate = 60;
      
      _boardView = Instantiate(_boardViewPrefab, Vector3.zero, quaternion.identity);
      _boardView.gameObject.SetActive(false);
      
      _gameOverView = Instantiate(_gameOverViewPrefab, Vector3.zero, quaternion.identity);
      _gameOverView.RegisterOnGameOverClicked(HandleGameOverClicked);
      _gameOverView.RegisterOnRestartClicked(HandleRestartClicked);
      _gameOverViewPrefab.gameObject.SetActive(false);
      
      StartNewGame();
   }
   
   private void StartNewGame()
   {
      BoardModel model = new BoardModel(_length, _width);
      _boardController = new BoardController(model, _boardView);
      model.RegisterGridChange(HandleTurnEnded);
      
      _currentState = GameStateType.Active;
      CurrentMarkerType = MarkerType.X;
      
      _gameOverView.gameObject.SetActive(false);
      _boardView.gameObject.SetActive(true);
      _boardView.Draw(_boardController.GetGrid());
   }
   
   private void HandleTurnEnded(MarkPlacedEvent markPlacedEvent)
   {
      MarkerType? winner = _boardController.TryGetWinner();
      if (winner == null && !_boardController.IsBoardFull())
      {
         if (CurrentMarkerType == MarkerType.X)
            CurrentMarkerType = MarkerType.O;
         else
            CurrentMarkerType = MarkerType.X;
         _boardView.Draw(markPlacedEvent.CurrentState);
         return;
      }
      
      _currentState = GameStateType.GameOver;
      _gameOverView.Draw(winner);
   }
   private void HandleGameOverClicked()
   {
      Debug.Log("ByeBye");
      Application.Quit();
      return;
   }
   private void HandleRestartClicked()
   {
      StartNewGame();
   }

   private void OnDestroy()
   {
      if (Instance != this)
      {
         return;
      }
      
      _gameOverView.UnregisterOnGameOverClicked(HandleGameOverClicked);
      _gameOverView.UnregisterOnRestartClicked(HandleRestartClicked);
   }
}
