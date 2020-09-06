using System;
using Common.ExecutionResults;
using Xunit;

namespace Common.Tests
{
    public sealed class ExecutionResultTests
    {
        [Fact]
        public void TestSuccessExecutionResult()
        {
            var executionResult = ExecutionResult.Success();
            Assert.True(executionResult.IsSuccess);
            Assert.Empty(executionResult.Errors);
            executionResult = ExecutionResult.Failed(new[] {"Error"});
            Assert.False(executionResult.IsSuccess);
            Assert.NotEmpty(executionResult.Errors);

            var intExecutionResult = ExecutionResult.Success(10);
            Assert.True(intExecutionResult.IsSuccess);
            Assert.Empty(intExecutionResult.Errors);
            Assert.Equal(10, intExecutionResult.SuccessValue);
            intExecutionResult = ExecutionResult.Failed<int>("Error");
            Assert.False(intExecutionResult.IsSuccess);
            Assert.NotEmpty(intExecutionResult.Errors);
        }

        [Fact]
        public void CreateFailureShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => ExecutionResult.Failed(null));
            Assert.Throws<ArgumentException>(() => ExecutionResult.Failed());
            Assert.Throws<ArgumentException>(() => ExecutionResult.Failed(Array.Empty<string>()));
            Assert.Throws<ArgumentNullException>(() => ExecutionResult.Failed<int>(null));
            Assert.Throws<ArgumentException>(() => ExecutionResult.Failed<int>());
            Assert.Throws<ArgumentException>(() => ExecutionResult.Failed<int>(Array.Empty<string>()));
        }
    }
}