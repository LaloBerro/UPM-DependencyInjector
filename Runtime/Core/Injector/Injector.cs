namespace DependencyInjector.Core
{
    public class Injector
    {
        private readonly IDIContainer _mainDiContainer;
        private readonly IDIContainer[] _othersDiContainers;
        private readonly IReflectionInjector[] _reflectionInjectors;
        private readonly IInstaller[] _installers;

        public Injector(IInstaller[] installers, IDIContainer mainDiContainer, IReflectionInjector[] reflectionInjectors, IDIContainer[] othersDiContainers)
        {
            _installers = installers;

            _reflectionInjectors = reflectionInjectors;
            _mainDiContainer = mainDiContainer;
            _othersDiContainers = othersDiContainers;
        }

        public void InjectAll()
        {
            foreach (var installer in _installers)
            {
                if(installer.HasToSkipInstallation())
                    continue;
                
                InjectValues(installer);
                
                installer.Install(_mainDiContainer);
            }
        }

        private void InjectValues(IInstaller installer)
        {
            int totalOthersDiContainers = _othersDiContainers?.Length ?? 0;
            int totalDiContainers = totalOthersDiContainers + 1;
            IDIContainer[] diContainers = new IDIContainer[totalDiContainers];

            diContainers[0] = _mainDiContainer;
            for (int i = 1; i < totalDiContainers; i++)
            {
                diContainers[i] = _othersDiContainers[i - 1];
            }
            
            foreach (var reflectionInjector in _reflectionInjectors)
            {
                reflectionInjector.Inject(diContainers ,installer);
            }
        }
    }
}