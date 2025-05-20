using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Common test methods for type extractors.
    /// </summary>
    public class TypesExtractorsCommonTests
    {
        #region CanHandle Tests

        public static void CanHandle_TypeCanNotBeHandled_ReturnFalse(ITypesExtractor extractor, Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            // Act
            var returnValue = extractor.CanHandle(type, typeExtractionOptions);

            // Assert
            Assert.IsFalse(returnValue);
        }

        public static void CanHandle_TypeCanBeHandled_ReturnTrue(ITypesExtractor extractor, Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            // Act
            var returnValue = extractor.CanHandle(type, typeExtractionOptions);

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion CanHandle Tests

        #region ExtractUsedTypes Tests

        public static void ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(ITypesExtractor extractor, Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var isArgumentExceptionThrown = false;

            // Act
            try
            {
                var actualUsedTypes = extractor.ExtractUsedTypes(type, typeExtractionOptions);
            }
            catch (ArgumentException)
            {
                isArgumentExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(isArgumentExceptionThrown);
        }

        public static void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(ITypesExtractor extractor, Type type, ITypeExtractionOptions typeExtractionOptions, IEnumerable<Type> expectedUsedTypes)
        {
            // Act
            var actualUsedTypes = extractor.ExtractUsedTypes(type, typeExtractionOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedUsedTypes.ToList(), actualUsedTypes.ToList());
        }

        #endregion ExtractUsedTypes Tests
    }
}
