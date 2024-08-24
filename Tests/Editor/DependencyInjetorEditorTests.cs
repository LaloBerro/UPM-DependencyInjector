using System;
using DependencyInjector.Core;
using DependencyInjector.Tests.BaseClasses;
using NUnit.Framework;

namespace DependencyInjector.EditorTests
{
    public class DependencyInjectorEditorTests
    {
        [Test]
        public void InjectAsSingle_CheckIfItsInjected()
        {
            IInstaller[] installers = { new SingleInjectThisInstaller(), new SingleInjectionTestInstaller() };
            IDIContainer diContainer = new DIContainer();

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(installers, diContainer, reflectionInjectors, null);
            injector.InjectAll();
            
            InjectionTest injectionTest = diContainer.Get<InjectionTest>();
            
            Assert.IsTrue(injectionTest.IsInjected());
        }
        
        [Test]
        public void InjectMultipleAsSingle_ThrowError()
        {
            IInstaller[] installers = { new SingleInjectThisInstaller(), new SingleInjectionTestInstaller(), new SingleInjectThisInstaller()};
            IDIContainer diContainer = new DIContainer();

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(installers, diContainer, reflectionInjectors, null);
            
            Assert.Throws<Exception>(() => {injector.InjectAll();});
        }

        [Test]
        public void InjectAsMultiple_CheckIfItsInjected()
        {
            IInstaller[] installers = { new MultipleInjectThisInstaller(), new MultipleInjectionTestInstaller() };
            IDIContainer diContainer = new DIContainer();

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(installers, diContainer, reflectionInjectors, null);
            injector.InjectAll();
            
            InjectionTest injectionTest = diContainer.Get<InjectionTest>();
            
            Assert.IsTrue(injectionTest.IsInjected());
        }
        
        [Test]
        public void InjectArrayAsMultiple_CheckIfItsInjected()
        {
            IInstaller[] installers = {new MultipleInjectThisInstaller(), new MultipleInjectThisInstaller(),new MultipleInjectThisInstaller(), new ArrayInjectionTestInstaller() };
            IDIContainer diContainer = new DIContainer();

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(installers, diContainer, reflectionInjectors, null);
            injector.InjectAll();
            
            ArrayInjectionTest arrayInjectionTest = diContainer.Get<ArrayInjectionTest>();
            
            Assert.IsTrue(arrayInjectionTest.IsInjected());
        }
        
        [Test]
        public void InjectAsSingle_UseOtherContext_CheckIfItsInjected()
        {
            IInstaller[] installers = { new SingleInjectThisInstaller()};
            IDIContainer diContainer = new DIContainer();
            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(installers, diContainer, reflectionInjectors, null);
            injector.InjectAll();
            
            IInstaller[] otherInstallers = { new SingleInjectionTestInstaller() };
            IDIContainer otherDiContainer = new DIContainer();
            IReflectionInjector[] otherReflectionInjectors = { new FieldsReflectionInjector() };
            Injector otherInjector = new Injector(otherInstallers, otherDiContainer, otherReflectionInjectors, new []{diContainer});
            otherInjector.InjectAll();
            
            InjectionTest injectionTest = otherDiContainer.Get<InjectionTest>();
            
            Assert.IsTrue(injectionTest.IsInjected());
        }
        
        [Test]
        public void InjectAsMultiple_UseOtherContext_CheckIfItsInjected()
        {
            IInstaller[] installers = { new MultipleInjectThisInstaller(), new MultipleInjectThisInstaller(), new MultipleInjectThisInstaller()};
            IDIContainer diContainer = new DIContainer();
            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(installers, diContainer, reflectionInjectors, null);
            injector.InjectAll();
            
            IInstaller[] otherInstallers = { new ArrayInjectionTestInstaller() };
            IDIContainer otherDiContainer = new DIContainer();
            IReflectionInjector[] otherReflectionInjectors = { new FieldsReflectionInjector() };
            Injector otherInjector = new Injector(otherInstallers, otherDiContainer, otherReflectionInjectors, new []{diContainer});
            otherInjector.InjectAll();
            
            ArrayInjectionTest arrayInjectionTest = otherDiContainer.Get<ArrayInjectionTest>();
            
            Assert.IsTrue(arrayInjectionTest .IsInjected());
        }
    }
}