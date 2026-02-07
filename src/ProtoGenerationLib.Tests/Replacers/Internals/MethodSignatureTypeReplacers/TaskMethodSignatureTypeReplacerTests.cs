using ProtoGenerationLib.Replacers.Internals.MethodSignatureTypeReplacers;

namespace ProtoGenerationLib.Tests.Replacers.Internals.MethodSignatureTypeReplacers
{
    [TestClass]
    public class TaskMethodSignatureTypeReplacerTests
    {
        private static TaskMethodSignatureTypeReplacer replacer;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            replacer = new TaskMethodSignatureTypeReplacer();
        }

        [DataRow(typeof(int), false)]
        [DataRow(typeof(Task), true)]
        [DataRow(typeof(Task<int>), true)]
        [DataTestMethod]
        public void CanReplace_ResultIsCorrect(Type type, bool expectedResult)
        {
            // Act
            var actual1 = replacer.CanReplace(type, true);
            var actual2 = replacer.CanReplace(type, false);

            // Assert
            Assert.AreEqual(expectedResult, actual1);
            Assert.AreEqual(expectedResult, actual2);
        }

        [TestMethod]
        public void ReplaceType_TypeCanNotBeHandled_ThrowArgumentException()
        {
            // Arrange
            var type = typeof(int);

            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => replacer.ReplaceType(type, true));
            Assert.ThrowsException<ArgumentException>(() => replacer.ReplaceType(type, false));
        }

        [TestMethod]
        public void ReplaceType_TypeIsTask_ReturnVoid()
        {
            // Arrange
            var type = typeof(Task);
            var expectedReplacerType = typeof(void);

            // Act
            var actualReplacerType1 = replacer.ReplaceType(type, true);
            var actualReplacerType2 = replacer.ReplaceType(type, false);

            // Assert
            Assert.AreEqual(expectedReplacerType, actualReplacerType1);
            Assert.AreEqual(expectedReplacerType, actualReplacerType2);
        }

        [TestMethod]
        public void ReplaceType_TypeIsGenericTask_ReturnTaskResultType()
        {
            // Arrange
            var type = typeof(Task<int>);
            var expectedReplacerType = typeof(int);

            // Act
            var actualReplacerType1 = replacer.ReplaceType(type, true);
            var actualReplacerType2 = replacer.ReplaceType(type, false);

            // Assert
            Assert.AreEqual(expectedReplacerType, actualReplacerType1);
            Assert.AreEqual(expectedReplacerType, actualReplacerType2);
        }

        [TestMethod]
        public void ReplaceType_TypeInheritsFromTask_ReturnVoid()
        {
            // Arrange
            var type = typeof(TaskEnhancer1);
            var expectedReplacerType = typeof(void);

            // Act
            var actualReplacerType1 = replacer.ReplaceType(type, true);
            var actualReplacerType2 = replacer.ReplaceType(type, false);

            // Assert
            Assert.AreEqual(expectedReplacerType, actualReplacerType1);
            Assert.AreEqual(expectedReplacerType, actualReplacerType2);
        }

        [TestMethod]
        public void ReplaceType_TypeInheritsFromGenericTask_ReturnTaskResultType()
        {
            // Arrange
            var type = typeof(TaskEnhancer2);
            var expectedReplacerType = typeof(int);

            // Act
            var actualReplacerType1 = replacer.ReplaceType(type, true);
            var actualReplacerType2 = replacer.ReplaceType(type, false);

            // Assert
            Assert.AreEqual(expectedReplacerType, actualReplacerType1);
            Assert.AreEqual(expectedReplacerType, actualReplacerType2);
        }

        private class TaskEnhancer1 : Task
        {
            public TaskEnhancer1(Action action) : base(action)
            {
            }

            public TaskEnhancer1(Action action, CancellationToken cancellationToken) : base(action, cancellationToken)
            {
            }

            public TaskEnhancer1(Action action, TaskCreationOptions creationOptions) : base(action, creationOptions)
            {
            }

            public TaskEnhancer1(Action<object?> action, object? state) : base(action, state)
            {
            }

            public TaskEnhancer1(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(action, cancellationToken, creationOptions)
            {
            }

            public TaskEnhancer1(Action<object?> action, object? state, CancellationToken cancellationToken) : base(action, state, cancellationToken)
            {
            }

            public TaskEnhancer1(Action<object?> action, object? state, TaskCreationOptions creationOptions) : base(action, state, creationOptions)
            {
            }

            public TaskEnhancer1(Action<object?> action, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(action, state, cancellationToken, creationOptions)
            {
            }
        }

        private class TaskEnhancer2 : Task<int>
        {
            public TaskEnhancer2(Func<int> function) : base(function)
            {
            }

            public TaskEnhancer2(Func<object?, int> function, object? state) : base(function, state)
            {
            }

            public TaskEnhancer2(Func<int> function, CancellationToken cancellationToken) : base(function, cancellationToken)
            {
            }

            public TaskEnhancer2(Func<int> function, TaskCreationOptions creationOptions) : base(function, creationOptions)
            {
            }

            public TaskEnhancer2(Func<object?, int> function, object? state, CancellationToken cancellationToken) : base(function, state, cancellationToken)
            {
            }

            public TaskEnhancer2(Func<object?, int> function, object? state, TaskCreationOptions creationOptions) : base(function, state, creationOptions)
            {
            }

            public TaskEnhancer2(Func<int> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, cancellationToken, creationOptions)
            {
            }

            public TaskEnhancer2(Func<object?, int> function, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, state, cancellationToken, creationOptions)
            {
            }
        }
    }
}
