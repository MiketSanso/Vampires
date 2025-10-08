using _Project.Scripts.Model;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TransformsModel>().AsSingle();
            Container.Bind<EnemyPool>().AsSingle();
        }
    }
}