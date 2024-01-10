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
            Injector injector = new Injector(installers, diContainer, reflectionInjectors);
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
            Injector injector = new Injector(installers, diContainer, reflectionInjectors);
            
            Assert.Throws<Exception>(() => {injector.InjectAll();});
        }

        [Test]
        public void InjectAsMultiple_CheckIfItsInjected()
        {
            IInstaller[] installers = { new MultipleInjectThisInstaller(), new MultipleInjectionTestInstaller() };
            IDIContainer diContainer = new DIContainer();

            IReflectionInjector[] reflectionInjectors = { new FieldsReflectionInjector() };
            Injector injector = new Injector(installers, diContainer, reflectionInjectors);
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
            Injector injector = new Injector(installers, diContainer, reflectionInjectors);
            injector.InjectAll();
            
            ArrayInjectionTest arrayInjectionTest = diContainer.Get<ArrayInjectionTest>();
            
            Assert.IsTrue(arrayInjectionTest.IsInjected());
        }
    }
}