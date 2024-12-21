using System;
using R3;

namespace Kiro.Domain
{
    public sealed class BookPageModel : IDisposable
    {
        public ReadOnlyReactiveProperty<Page> CurrentLeftPage => _currentLeftPage;
        readonly ReactiveProperty<Page> _currentLeftPage = new(new Page(0));

        public void Set(Page page)
        {
            _currentLeftPage.Value = page;
        }

        public void Dispose()
        {
            _currentLeftPage?.Dispose();
        }
    }
}