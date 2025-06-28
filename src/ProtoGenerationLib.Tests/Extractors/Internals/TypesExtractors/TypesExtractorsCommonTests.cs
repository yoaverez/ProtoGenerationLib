using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Common test methods for type extractors.
    /// </summary>
    public class TypesExtractorsCommonTests
    {
        #region CanHandle Tests

        internal static void CanHandle_TypeCanNotBeHandled_ReturnFalse(ITypesExtractor extractor, Type type, IProtoGenerationOptions generationOptions)
        {
            // Act
            var returnValue = extractor.CanHandle(type, generationOptions);

            // Assert
            Assert.IsFalse(returnValue);
        }

        internal static void CanHandle_TypeCanBeHandled_ReturnTrue(ITypesExtractor extractor, Type type, IProtoGenerationOptions generationOptions)
        {
            // Act
            var returnValue = extractor.CanHandle(type, generationOptions);

            // Assert
            Assert.IsTrue(returnValue);
        }

        internal static void CanHandle_TypeCanNotBeHandled_ReturnFalse(IWrapperElementTypeExtractor extractor, Type type)
        {
            // Act
            var returnValue = extractor.CanHandle(type);

            // Assert
            Assert.IsFalse(returnValue);
        }

        internal static void CanHandle_TypeCanBeHandled_ReturnTrue(IWrapperElementTypeExtractor extractor, Type type)
        {
            // Act
            var returnValue = extractor.CanHandle(type);

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion CanHandle Tests

        #region ExtractUsedTypes Tests

        internal static void ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(ITypesExtractor extractor, Type type, IProtoGenerationOptions generationOptions)
        {
            var isArgumentExceptionThrown = false;

            // Act
            try
            {
                var actualUsedTypes = extractor.ExtractUsedTypes(type, generationOptions);
            }
            catch (ArgumentException)
            {
                isArgumentExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(isArgumentExceptionThrown);
        }

        internal static void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(ITypesExtractor extractor, Type type, IProtoGenerationOptions generationOptions, IEnumerable<Type> expectedUsedTypes)
        {
            // Act
            var actualUsedTypes = extractor.ExtractUsedTypes(type, generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedUsedTypes.ToList(), actualUsedTypes.ToList());
        }

        internal static void ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(IWrapperElementTypeExtractor extractor, Type type)
        {
            var isArgumentExceptionThrown = false;

            // Act
            try
            {
                var actualUsedTypes = extractor.ExtractUsedTypes(type);
            }
            catch (ArgumentException)
            {
                isArgumentExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(isArgumentExceptionThrown);
        }

        internal static void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(IWrapperElementTypeExtractor extractor, Type type, IEnumerable<Type> expectedUsedTypes)
        {
            // Act
            var actualUsedTypes = extractor.ExtractUsedTypes(type);

            // Assert
            CollectionAssert.AreEquivalent(expectedUsedTypes.ToList(), actualUsedTypes.ToList());
        }

        #endregion ExtractUsedTypes Tests
    }
}
