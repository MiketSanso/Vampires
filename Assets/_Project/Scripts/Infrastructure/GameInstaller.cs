using _Project.Scripts.Model;
using _Project.Scripts.ViewModel;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ChatViewModel>().AsSingle();
            Container.Bind<GameStateModel>().AsSingle();
            Container.Bind<TransformsModel>().AsSingle();
        }
    }
}