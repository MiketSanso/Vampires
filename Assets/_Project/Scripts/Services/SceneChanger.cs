using _Project.Scripts.ScriptableObjects;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services
{
    public class SceneChanger
    {
        public readonly SceneNumbData SceneNumbDat;

        public SceneChanger(SceneNumbData sceneNumbData)
        {
            SceneNumbDat = sceneNumbData;
        }
        
        public void ChangeScene(int numberScene)
        {
            SceneManager.LoadScene(numberScene);
        }
    }
}