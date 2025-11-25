using System;
using R3;

namespace _Project.Scripts.ViewModel
{
    public interface IConnectViewModel
    {
        public ReactiveCommand<Unit> ConnectAsHostCommand { get; }
        public ReactiveCommand<string> ConnectAsPlayerCommand { get; }
        public ReactiveCommand<string> SetNameCommand { get; }
        public ReactiveCommand<Unit> ShowPanelCodeCommand { get; }
        public Observable<Unit> OnPanelActivated { get; }
    }
}