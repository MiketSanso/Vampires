using System.Collections.Generic;
using R3;

namespace _Project.Scripts.ViewModel
{
    public interface IChatViewModel
    {
        public ReactiveCommand<string> AddMessageCommand { get; }
        public Observable<Queue<string>> Messages { get; }
    }
}