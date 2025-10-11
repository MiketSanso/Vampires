namespace _Project.Scripts.Model
{
    public class GameStateModel
    {
        public bool IsGameActive { get; private set; }

        public void EndGame()
        {
            IsGameActive = false;
        }

        public void StartGame()
        {
            IsGameActive = true;
        }
    }
}