using System;
using System.Linq;
using System.Text;
using Board.BoardState;
using Board.Model;
using Board.View.Abstract;
using GameState;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Board.View.Implementation
{
    public class TextBoardView : BaseBoardView
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _submitButton;
        
        private void Awake()
        {
            _submitButton.onClick.AddListener(GetPlayerInput);
        }
        
        private void OnDestroy()
        {
            _submitButton.onClick.RemoveListener(GetPlayerInput);
        }
        
        protected override Vector2Int? TryGetPlayerInputInternal()
        {
            string input = _inputField.text;
            string[] split = input.Split(',');
            
            if (split.Length != 2)
            {
                Debug.Log("Invalid input, please enter two numbers separated by a comma");
                return null;
            }
            if (!int.TryParse(split[0], out int x) || !int.TryParse(split[1], out int y))
            {
                Debug.Log("Invalid input, please enter two numbers separated by a comma");
                return null;
            }

            _inputField.text = string.Empty;
            return new Vector2Int(x, y);
        }

        protected override void DrawBoardInternal(MarkerType?[,] grid)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("~~~~~~~ Tic Tac Toe ~~~~~~~");
            ViewManager.GetInstance().LoadView(gameObject);
            stringBuilder.AppendLine($"please choose Cordinets");
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);
            stringBuilder.AppendLine();
            
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"\t  {string.Join(" ",Enumerable.Range(0,columns))}");
            for (int row = 0; row < rows; row++)
            {
                stringBuilder.Append($"{row}\t");
                for (int col = 0; col < columns; col++)
                {
                    string cellValue = grid[row, col] == null ? "~" : grid[row, col].ToString();
                    stringBuilder.Append($"{cellValue}");
                    if (col < rows - 1)
                        stringBuilder.Append("|");
                }
                stringBuilder.AppendLine();
                if (row < rows - 1)
                    stringBuilder.AppendLine("\t -----------");
            }
            Debug.Log(stringBuilder.ToString());
        }
    }
}