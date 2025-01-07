using UnityEngine;

namespace GameState
{
    public class ViewManager
    {
        private static ViewManager Instance;
        
        private GameObject _currentScene;
        
        private ViewManager()
        {
        }
        
        public static ViewManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new ViewManager();
            }

            return Instance;
        }
        
        public void LoadView(GameObject scene)
        {
            if (_currentScene != null)
            {
                _currentScene.SetActive(false);
            }
            _currentScene = scene;
            _currentScene.SetActive(true);
        }
        
        
    }
}