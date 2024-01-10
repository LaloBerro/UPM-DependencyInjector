namespace DependencyInjector.Core
{
    public class Injector
    {
        private readonly IDIContainer _diContainer;
        private readonly IReflectionInjector[] _reflectionInjectors;
        private readonly IInstaller[] _installers;

        public Injector(IInstaller[] installers, IDIContainer diContainer, IReflectionInjector[] reflectionInjectors)
        {
            _installers = installers;

            _reflectionInjectors = reflectionInjectors;
            _diContainer = diContainer;
        }

        public void InjectAll()
        {
            foreach (var installer in _installers)
            {
                InjectValues(installer);
                
                installer.Install(_diContainer);
            }
        }

        private void InjectValues(IInstaller installer)
        {
            foreach (var reflectionInjector in _reflectionInjectors)
            {
                reflectionInjector.Inject(_diContainer ,installer);
            }
        }
    }
}