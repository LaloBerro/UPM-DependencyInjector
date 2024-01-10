namespace DependencyInjector.Tests.BaseClasses
{
    public class InjectionTest
    {
        private readonly IInjectThis _injectThis;

        public InjectionTest(IInjectThis injectThis)
        {
            _injectThis = injectThis;
        }

        public bool IsInjected()
        {
            return _injectThis.IsInjected();
        }
    }
}