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
            executionResult = ExecutionResult.Failure(new[] {"Error"});
            Assert.False(executionResult.IsSuccess);
            Assert.NotEmpty(executionResult.Errors);

            var intExecutionResult = ExecutionResult.Success(10);
            Assert.True(intExecutionResult.IsSuccess);
            Assert.Empty(intExecutionResult.Errors);
            Assert.Equal(10, intExecutionResult.SuccessValue);
            intExecutionResult = ExecutionResult.Failure<int>("Error");
            Assert.False(intExecutionResult.IsSuccess);
            Assert.NotEmpty(intExecutionResult.Errors);
        }

        [Fact]
        public void CreateFailureShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => ExecutionResult.Failure(null));
            Assert.Throws<ArgumentException>(() => ExecutionResult.Failure());
            Assert.Throws<ArgumentException>(() => ExecutionResult.Failure(Array.Empty<string>()));
            Assert.Throws<ArgumentNullException>(() => ExecutionResult.Failure<int>(null));
            Assert.Throws<ArgumentException>(() => ExecutionResult.Failure<int>());
            Assert.Throws<ArgumentException>(() => ExecutionResult.Failure<int>(Array.Empty<string>()));
        }
    }
}