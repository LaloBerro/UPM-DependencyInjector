namespace DependencyInjector.Tests.BaseClasses
{
    public class ArrayInjectionTest
    {
        private readonly IInjectThis[] _injectThis;

        public ArrayInjectionTest(IInjectThis[] injectThis)
        {
            _injectThis = injectThis;
        }

        public bool IsInjected()
        {
            foreach (var injectThis in _injectThis)
            {
                if (!injectThis.IsInjected())
                    return false;
            }

            return true;
        }
    }
}