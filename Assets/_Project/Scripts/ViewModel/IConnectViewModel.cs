using R3;

namespace _Project.Scripts.ViewModel
{
    public interface IConnectViewModel
    {
        public ReactiveCommand<Unit> ConnectAsHostCommand { get; }
        public ReactiveCommand<Unit> ConnectAsPlayerCommand { get; }
    }
}