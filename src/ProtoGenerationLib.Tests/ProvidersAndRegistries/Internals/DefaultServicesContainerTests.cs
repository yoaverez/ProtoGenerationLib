using Castle.DynamicProxy.Internal;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib.ProvidersAndRegistries.Internals;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;
using ProtoGenerationLib.Utilities.TypeUtilities;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals
{
    [TestClass]
    public class DefaultServicesContainerTests
    {
        private static ISet<Type> strategiesInterfaces;

        private static ISet<Type> concreteStrategies;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            strategiesInterfaces = typeof(IFileNamingStrategy).Assembly.GetTypes()
                                                                       .Where(t => t.IsInterface && t.Namespace.Contains(nameof(Strategies)))
                                                                       .ToHashSet();

            concreteStrategies = typeof(SingleFileStrategy).Assembly.GetTypes()
                                                                    .Where(t => !t.IsInterface && t.GetAllImplementedInterfaces().Any(interfaceType => strategiesInterfaces.Contains(interfaceType)))
                                                                    .ToHashSet();
        }

        [DynamicData(nameof(GetStrategiesTypesAndNames), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CreateDefaultServicesContainer_ContainsAllTheProjectDefinedStrategiesThatImplementsTheTestedStrategy(Type testedStrategy, IEnumerable<string> strategiesNames)
        {
            // Arrange
            var providerInterfaceTypes = typeof(IProvider).GetAllImplementedInterfaces().Append(typeof(IProvider));
            var providerStrategyMethod = providerInterfaceTypes.SelectMany(t => t.GetMethods().Where(m => m.ReturnType.Equals(testedStrategy))).Single();

            var actualConcreteTypes = new HashSet<Type>();
            var expectedConcreteTypes = concreteStrategies.Where(strategyType => strategyType.GetAllInterfaces().Contains(testedStrategy))
                                                          .ToHashSet();

            // Act
            foreach (var name in strategiesNames)
            {
                var strategy = providerStrategyMethod.Invoke(DefaultServicesContainer.Instance, new object[] { name });
                actualConcreteTypes.Add(strategy.GetType());
            }

            // Assert
            CollectionAssert.AreEquivalent(expectedConcreteTypes.ToArray(), actualConcreteTypes.ToArray());
        }

        [TestMethod]
        public void CheckThatAllStrategiesAreTested()
        {
            // Arrange + Act
            var testedStrategies = GetStrategiesTypesAndNames().Select(x => x[0] as Type).ToArray();

            // Assert
            CollectionAssert.AreEquivalent(strategiesInterfaces.ToArray(), testedStrategies);
        }

        private static IEnumerable<object[]> GetStrategiesTypesAndNames()
        {
            return new List<object[]>
            {
                new object[] { typeof(IEnumValueNumberingStrategy), StrategyNamesLookup.EnumValueNumberingStrategiesLookup.Values },
                new object[] { typeof(IFieldNumberingStrategy), StrategyNamesLookup.FieldNumberingStrategiesLookup.Values },
                new object[] { typeof(IFieldsAndPropertiesExtractionStrategy), StrategyNamesLookup.FieldsAndPropertiesExtractionStrategiesLookup.Values },
                new object[] { typeof(IDocumentationExtractionStrategy), StrategyNamesLookup.DocumentationExtractionStrategiesLookup.Values },
                new object[] { typeof(IMethodSignatureExtractionStrategy), StrategyNamesLookup.MethodSignatureExtractionStrategiesLookup.Values },
                new object[] { typeof(IFileNamingStrategy), StrategyNamesLookup.FilePathStrategiesLookup.Values },
                new object[] { typeof(INewTypeNamingStrategy), StrategyNamesLookup.NewTypeNamingStrategiesLookup.Values },
                new object[] { typeof(IPackageNamingStrategy), StrategyNamesLookup.PackageNamingStrategiesLookup.Values },
                new object[] { typeof(IParameterListNamingStrategy), StrategyNamesLookup.ParameterListNamingStrategiesLookup.Values },
                new object[] { typeof(IProtoStylingStrategy), StrategyNamesLookup.ProtoStylingStrategiesLookup.Values },
                new object[] { typeof(IPackageStylingStrategy), StrategyNamesLookup.PackageStylingStrategiesLookup.Values },
                new object[] { typeof(ITypeNamingStrategy), StrategyNamesLookup.TypeNamingStrategiesLookup.Values },
                new object[] { typeof(IFilePathStylingStrategy), StrategyNamesLookup.FilePathStylingStrategiesLookup.Values },
            };
        }
    }
}
