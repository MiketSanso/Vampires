using _Project.Scripts.ScriptableObjects;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services
{
    public class SceneChanger
    {
        public readonly SceneNumbData SceneNumbData;

        public SceneChanger(SceneNumbData sceneNumbData)
        {
            SceneNumbData = sceneNumbData;
        }
        
        public void ChangeScene(int numberScene)
        {
            SceneManager.LoadScene(numberScene);
        }
    }
}