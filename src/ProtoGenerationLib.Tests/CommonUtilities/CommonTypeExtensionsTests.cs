using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.CommonUtilities;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Tests.CommonUtilities.DummyTypes;
using System.Reflection;

namespace ProtoGenerationLib.Tests.CommonUtilities
{
    [TestClass]
    public class CommonTypeExtensionsTests
    {
        private ProtoGenerationOptions generationOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGenerationOptions
            {
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoServiceAttribute = typeof(ProtoServiceAttribute),
                    IsProtoServiceDelegate = (type) => false,
                    ProtoRpcAttribute = typeof(ProtoRpcAttribute),
                    TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) => { rpcType = ProtoRpcType.Unary; return false; },
                }
            };
        }

        #region IsProtoService Tests

        [TestMethod]
        public void IsProtoService_TypeHasTheAttribute_ReturnTrue()
        {
            // Arrange
            var testedType = typeof(AbstractService);

            // Act
            var result = testedType.IsProtoService(generationOptions.AnalysisOptions);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsProtoService_TypeHasTheAttributeWhichIsNotTheDefaultAttribute_ReturnTrue()
        {
            // Arrange
            var testedType = typeof(Service3);

            generationOptions.AnalysisOptions.ProtoServiceAttribute = typeof(ObsoleteAttribute);

            // Act
            var result = testedType.IsProtoService(generationOptions.AnalysisOptions);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsProtoService_IsServiceDelegateReturnTrueForType_ReturnTrue()
        {
            // Arrange
            var testedType = typeof(Service1);

            generationOptions.AnalysisOptions.IsProtoServiceDelegate = (type) =>
            {
                if (type.Equals(testedType))
                    return true;
                return false;
            };

            // Act
            var result = testedType.IsProtoService(generationOptions.AnalysisOptions);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsProtoService_IsServiceDelegateReturnTrueForTypeAndTypeHasAttribute_ReturnTrue()
        {
            // Arrange
            var testedType = typeof(Service2);

            generationOptions.AnalysisOptions.IsProtoServiceDelegate = (type) =>
            {
                if (type.Equals(testedType))
                    return true;
                return false;
            };

            // Act
            var result = testedType.IsProtoService(generationOptions.AnalysisOptions);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsProtoService_IsServiceDelegateReturnFalseForTypeAndTypeDoesNotHaveAttribute_ReturnFalse()
        {
            // Arrange
            var testedType = typeof(Service3);

            // Act
            var result = testedType.IsProtoService(generationOptions.AnalysisOptions);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion IsProtoService Tests

        #region GetMethodRpcType Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GetMethodRpcType_MethodShouldBeRpcByAttributeButAttributeIsNotTheDefaultOne_ThrowsArgumentException()
        {
            // Arrange
            var methodDeclaringType = typeof(IService5);
            var testedMethod = methodDeclaringType.GetMethod(nameof(IService5.IService5Method1))!;

            generationOptions.AnalysisOptions.ProtoRpcAttribute = typeof(ObsoleteAttribute);

            // Act
            testedMethod.GetMethodRpcType(methodDeclaringType, generationOptions.AnalysisOptions);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GetMethodRpcType_MethodIsNotRpc_ThrowsArgumentException()
        {
            // Arrange
            var methodDeclaringType = typeof(IService1);
            var testedMethod = methodDeclaringType.GetMethod(nameof(IService1.IService1Method2))!;

            // Act
            testedMethod.GetMethodRpcType(methodDeclaringType, generationOptions.AnalysisOptions);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [TestMethod]
        public void GetMethodRpcType_MethodIsRpcByAttribute_ReturnCorrectRpcType()
        {
            // Arrange
            var methodDeclaringType = typeof(Service2);
            var testedMethod = methodDeclaringType.GetMethod(nameof(Service2.IService5Method1))!;
            var expectedRpcType = ProtoRpcType.ServerStreaming;

            // Act
            var actualRpcType = testedMethod.GetMethodRpcType(methodDeclaringType, generationOptions.AnalysisOptions);

            // Assert
            Assert.AreEqual(expectedRpcType, actualRpcType);
        }

        [TestMethod]
        public void GetMethodRpcType_MethodIsRpcByDelegate_ReturnCorrectRpcType()
        {
            // Arrange
            var methodDeclaringType = typeof(IService5);
            var testedMethod = methodDeclaringType.GetMethod(nameof(IService5.IService5Method1))!;
            var expectedRpcType = ProtoRpcType.BidirectionalStreaming;

            generationOptions.AnalysisOptions.TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) =>
            {
                rpcType = ProtoRpcType.ServerStreaming;

                if (method.Name.Equals(nameof(IService5.IService5Method1)))
                {
                    rpcType = expectedRpcType;
                    return true;
                }

                return false;
            };

            // Act
            var actualRpcType = testedMethod.GetMethodRpcType(methodDeclaringType, generationOptions.AnalysisOptions);

            // Assert
            Assert.AreEqual(expectedRpcType, actualRpcType);
        }

        [TestMethod]
        public void GetMethodRpcType_MethodIsRpcByDelegateAndByAttribute_ReturnCorrectRpcType()
        {
            // Arrange
            var methodDeclaringType = typeof(IService1);
            var testedMethod = methodDeclaringType.GetMethod(nameof(IService1.IService1Method1))!;
            var expectedPossibleRpcTypes = new List<ProtoRpcType>
            {
                ProtoRpcType.BidirectionalStreaming ,
                ProtoRpcType.Unary ,
            };

            generationOptions.AnalysisOptions.TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) =>
            {
                rpcType = ProtoRpcType.ServerStreaming;

                if (method.Name.Equals(nameof(IService1.IService1Method1)))
                {
                    rpcType = ProtoRpcType.BidirectionalStreaming;
                    return true;
                }

                return false;
            };

            // Act
            var actualRpcType = testedMethod.GetMethodRpcType(methodDeclaringType, generationOptions.AnalysisOptions);

            // Assert
            CollectionAssert.Contains(expectedPossibleRpcTypes, actualRpcType);
        }

        [TestMethod]
        public void GetMethodRpcType_MethodIsRpcByImplementedInterface_ReturnCorrectRpcType()
        {
            // Arrange
            var methodDeclaringType = typeof(Service2);
            var testedMethod = methodDeclaringType.GetMethod(nameof(IService1.IService1Method1))!;
            var expectedRpcType = ProtoRpcType.Unary;

            // Act
            var actualRpcType = testedMethod.GetMethodRpcType(methodDeclaringType, generationOptions.AnalysisOptions);

            // Assert
            Assert.AreEqual(expectedRpcType, actualRpcType);
        }

        #endregion GetMethodRpcType Tests

        #region ExtractRpcMethods Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExtractRpcMethods_AttributeTypeIsNotAnAttribute_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(Service1);
            generationOptions.AnalysisOptions.ProtoRpcAttribute = typeof(int);

            // Act
            var result = type.ExtractRpcMethods(generationOptions.AnalysisOptions);

            // Assert
            // Noting to do. The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ExtractRpcMethods_TypeIsInterface_GetAllImplementedInterfacesMethods()
        {
            // Arrange
            var type = typeof(IService4);

            // Check that the condition is attribute or this method.
            generationOptions.AnalysisOptions.TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) =>
            {
                rpcType = ProtoRpcType.ServerStreaming;

                if (method.Name.Equals(nameof(IService4.IService1Method1)) ||
                    method.Name.Equals(nameof(IService4.IService1Method2)))
                    return true;

                return false;
            };

            var expectedMethodsNames = new List<string>
            {
                nameof(IService4.IService1Method1),
                nameof(IService4.IService1Method2),
                nameof(IService4.IService2Method1),
                nameof(IService4.IService3Method1),
                nameof(IService4.IService4Method1),
            };

            // Act
            var methods = type.ExtractRpcMethods(generationOptions.AnalysisOptions);
            var actualMethodsNames = methods.Select(method => method.Name).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedMethodsNames, actualMethodsNames);
        }

        [TestMethod]
        public void ExtractRpcMethods_AttributeIsNotProtoRpcAttribute_GetMethodsWithAttribute()
        {
            // Arrange
            var type = typeof(IService5);

            generationOptions.AnalysisOptions.ProtoRpcAttribute = typeof(ObsoleteAttribute);

            var expectedMethodsNames = new List<string>
            {
                nameof(IService5.IService5Method1),
            };

            // Act
            var methods = type.ExtractRpcMethods(generationOptions.AnalysisOptions);
            var actualMethodsNames = methods.Select(method => method.Name).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedMethodsNames, actualMethodsNames);
        }

        /// <summary>
        /// This test check multiple things:
        /// <list type="number">
        /// <item>Inherited methods are also check.</item>
        /// <item>
        /// If a method of the type is not considered rpc by itself but
        /// the method was declared because of implemented interface method that is
        /// considered rpc, then the method will be considered an rpc.
        /// </item>
        /// <item>
        /// If a method of the type is considered rpc by itself but
        /// the method was declared because of implemented interface method that is
        /// not considered rpc, then the method will still be considered an rpc.
        /// </item>
        /// </list>
        /// </summary>
        [TestMethod]
        public void ExtractRpcMethods_TypeIsDataTypeSomeMethodsInDataTypeAreNotRpcTheSameInterfaceMethodsAreAndViceVersa_GetAllMethods()
        {
            // Arrange
            var type = typeof(Service2);

            // Check that the condition is attribute or this method.
            generationOptions.AnalysisOptions.TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) =>
            {
                rpcType = ProtoRpcType.ServerStreaming;

                if (method.Name.Equals(nameof(IService1.IService1Method1)) ||
                    method.Name.Equals(nameof(IService1.IService1Method2)))
                    return true;

                return false;
            };

            var expectedMethodsNames = new List<string>
            {
                nameof(Service2.IService1Method1),
                nameof(Service2.IService1Method2),
                nameof(Service2.IService5Method1),
                nameof(Service1.Foo),
                nameof(AbstractService.Method1),
                nameof(AbstractService.Method2),
            };

            // Act
            var methods = type.ExtractRpcMethods(generationOptions.AnalysisOptions);
            var actualMethodsNames = methods.Select(method => method.Name).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedMethodsNames, actualMethodsNames);
        }

        #endregion ExtractRpcMethods Tests
    }
}
