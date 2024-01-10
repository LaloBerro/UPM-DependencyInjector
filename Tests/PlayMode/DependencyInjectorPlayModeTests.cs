using System;
using System.Collections;
using DependencyInjector.Installers;
using DependencyInjector.Tests.BaseClasses;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DependencyInjector.PlayMode
{
    public class DependencyInjectorPlayModeTests
    {
        [UnityTest]
        public IEnumerator InjectSingleAndMultipleOneTime_CheckIfItsInjected()
        {
            //Arrange
            MonoInstaller<InjectionTest> injectionTestSingleMonoInstaller = new GameObject("InjectionTestSingleMonoInstaller").AddComponent<InjectionTestSingleMonoInstaller>();
            MonoInstaller<IInjectThis> injectThisMultipleMonoInstaller = new GameObject("InjectThisMultipleMonoInstaller").AddComponent<InjectThisMultipleMonoInstaller>();
            MonoInjector monoInjector = new GameObject("MonoInjector").AddComponent<MonoInjector>();

            monoInjector.SetInstallers(new MonoInstaller[] { injectThisMultipleMonoInstaller, injectionTestSingleMonoInstaller });
            
            //Act
            monoInjector.InjectAll();

            //Assert
            bool isInjected = injectionTestSingleMonoInstaller.ServiceInstance.IsInjected();
            Assert.IsTrue(isInjected);

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator InjectSingleMultipleTimes_ThrowError()
        {
            //Arrange
            MonoInstaller<InjectionTest> injectionTestSingleMonoInstaller = new GameObject("InjectionTestSingleMonoInstaller").AddComponent<InjectionTestSingleMonoInstaller>();
            MonoInstaller<IInjectThis> injectThisMultipleMonoInstaller = new GameObject("InjectThisMultipleMonoInstaller").AddComponent<InjectThisMultipleMonoInstaller>();
            MonoInjector monoInjector = new GameObject("MonoInjector").AddComponent<MonoInjector>();

            monoInjector.SetInstallers(new MonoInstaller[] { injectThisMultipleMonoInstaller, injectionTestSingleMonoInstaller, injectionTestSingleMonoInstaller });

            //Act / Assert 

            Assert.Throws<Exception>(() => { monoInjector.InjectAll(); });

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator InjectMultiple_CheckIfItsInjected()
        {
            //Arrange
            MonoInstaller<ArrayInjectionTest> injectionTestSingleMonoInstaller = new GameObject("ArrayInjectionTestSingleMonoInstaller").AddComponent<ArrayInjectionTestSingleMonoInstaller>();
            MonoInstaller<IInjectThis> injectThisMultipleMonoInstaller = new GameObject("InjectThisMultipleMonoInstaller").AddComponent<InjectThisMultipleMonoInstaller>();
            MonoInjector monoInjector = new GameObject("MonoInjector").AddComponent<MonoInjector>();

            monoInjector.SetInstallers(new MonoInstaller[]
            {
                injectThisMultipleMonoInstaller, injectThisMultipleMonoInstaller, injectThisMultipleMonoInstaller,
                injectionTestSingleMonoInstaller
            });
            
            //Act
            monoInjector.InjectAll();

            //Assert
            bool isInjected = injectionTestSingleMonoInstaller.ServiceInstance.IsInjected();
            Assert.IsTrue(isInjected);

            yield return null;
        }
        
        [UnityTest]
        public IEnumerator InjectUsingBridgeInstaller_CheckIfItsInjected()
        {
            //Arrange / Act
            MonoInstaller<IInjectThis> injectThisMultipleMonoInstaller = new GameObject("InjectThisMultipleMonoInstaller").AddComponent<InjectThisMultipleMonoInstaller>();
            MonoInjector monoInjector = new GameObject("MonoInjector").AddComponent<MonoInjector>();
            monoInjector.SetInstallers(new MonoInstaller[] { injectThisMultipleMonoInstaller });
            
            MonoInstaller<InjectionTest> injectionTestSingleMonoInstaller = new GameObject("InjectionTestSingleMonoInstaller").AddComponent<InjectionTestSingleMonoInstaller>();
            BridgeMonoInstaller<IInjectThis> bridgeMonoInstaller = new GameObject("InjectThisBridgeMonoInstaller").AddComponent<InjectThisBridgeMonoInstaller>();
            bridgeMonoInstaller.SetInstaller(injectThisMultipleMonoInstaller);
            
            MonoInjector monoInjector2 = new GameObject("MonoInjector2").AddComponent<MonoInjector>();
            monoInjector2.SetInstallers(new MonoInstaller[] { bridgeMonoInstaller, injectionTestSingleMonoInstaller });
            
            InjectorsInitializer injectorsInitializer = new GameObject("InjectorsInitializer").AddComponent<InjectorsInitializer>();
            injectorsInitializer.SetInjectors(new BaseMonoInjector[] { monoInjector, monoInjector2 });
            
            //Act
            injectorsInitializer.InjectAll();
            
            //Assert
            bool isInjected = injectionTestSingleMonoInstaller.ServiceInstance.IsInjected();
            Assert.IsTrue(isInjected);

            yield return null;
        }
    }
}