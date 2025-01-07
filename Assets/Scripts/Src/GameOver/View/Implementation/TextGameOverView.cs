using Board.BoardState;
using Board.View.Abstract;
using GameOver.View.Abstract;
using UnityEngine;

namespace GameOver.View.Implementation
{
    public class TextGameOverView : BaseGameOverView
    {

        protected override void DrawInternal(MarkerType? winner)
        {
            if (winner != null)
            {
                Debug.Log("Winner is " + winner);
            }
            else
            {
                Debug.Log("Draw");
            }
            Debug.Log("Game Over");
            InvokeOnGameOverClicked();
        }
    }
}