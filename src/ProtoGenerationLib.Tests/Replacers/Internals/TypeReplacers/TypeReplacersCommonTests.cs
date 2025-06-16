using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Replacers.Abstracts;

namespace ProtoGenerationLib.Tests.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Common test methods for type replacers.
    /// </summary>
    public class TypeReplacersCommonTests
    {
        #region CanReplaceType Tests

        public static void CanReplaceType_TypeCanNotBeReplaced_ReturnFalse(ITypeReplacer replacer, Type type)
        {
            // Act
            var returnValue = replacer.CanReplaceType(type);

            // Assert
            Assert.IsFalse(returnValue);
        }

        public static void CanReplaceType_TypeCanBeReplaced_ReturnTrue(ITypeReplacer replacer, Type type)
        {
            // Act
            var returnValue = replacer.CanReplaceType(type);

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion CanReplaceType Tests

        #region ReplaceType Tests

        public static void ReplaceType_TypeCanNotBeReplaced_ThrowsArgumentException(ITypeReplacer replacer, Type type, IProtoGenerationOptions generationOptions)
        {
            var isArgumentExceptionThrown = false;

            // Act
            try
            {
                var actualNewType = replacer.ReplaceType(type, generationOptions);
            }
            catch (ArgumentException)
            {
                isArgumentExceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(isArgumentExceptionThrown);
        }

        public static void ReplaceType_TypeCanBeReplaced_ReturnNewType(ITypeReplacer replacer, Type type, IProtoGenerationOptions generationOptions, string expectedNewType)
        {
            // Act
            var actualNewType = replacer.ReplaceType(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedNewType, actualNewType.Name);
        }

        #endregion ReplaceType Tests
    }
}
