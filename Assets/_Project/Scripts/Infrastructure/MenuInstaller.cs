using Zenject;

namespace _Project.Scripts.Infrastructure
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ConnectViewModel>().AsSingle();
        }
    }
}