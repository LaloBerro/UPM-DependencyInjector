namespace DependencyInjector.Tests.BaseClasses
{
    public class InjectThis : IInjectThis
    {
        public bool IsInjected()
        {
            return true;
        }
    }
}