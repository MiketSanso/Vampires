using _Project.Scripts.Model;
using _Project.Scripts.ViewModel;
using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MessagesModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChatViewModel>().AsSingle();
        }
    }
}