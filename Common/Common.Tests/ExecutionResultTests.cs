using System;
using Common.ExecutionResults;
using Xbehave;
using Xunit;

namespace Common.Tests
{
    public sealed class ExecutionResultTests
    {
        [ Fact ]
        public void TestSuccessExecutionResult()
        {
            var executionResult = ExecutionResult.Success();
            Assert.True( executionResult.IsSuccess );
            Assert.Empty( executionResult.Errors );
            executionResult = ExecutionResult.Failed( new[] { "Error" } );
            Assert.False( executionResult.IsSuccess );
            Assert.NotEmpty( executionResult.Errors );

            var intExecutionResult = ExecutionResult.Success( 10 );
            Assert.True( intExecutionResult.IsSuccess );
            Assert.Empty( intExecutionResult.Errors );
            Assert.Equal( 10, intExecutionResult.SuccessValue );
            intExecutionResult = ExecutionResult.Failed<int>( "Error" );
            Assert.False( intExecutionResult.IsSuccess );
            Assert.NotEmpty( intExecutionResult.Errors );
        }

        [ Fact ]
        public void CreateFailureShouldFail()
        {
            Assert.Throws<ArgumentNullException>( () => ExecutionResult.Failed( null ) );
            Assert.Throws<ArgumentException>( () => ExecutionResult.Failed() );
            Assert.Throws<ArgumentException>( () => ExecutionResult.Failed( Array.Empty<string>() ) );
            Assert.Throws<ArgumentNullException>( () => ExecutionResult.Failed<int>( null ) );
            Assert.Throws<ArgumentException>( () => ExecutionResult.Failed<int>() );
            Assert.Throws<ArgumentException>( () => ExecutionResult.Failed<int>( Array.Empty<string>() ) );
        }

        [ Scenario ]
        public void Map()
        {
            IExecutionResult<int> r1        = null;
            Func<int, int>        map1      = null;
            IExecutionResult<int> applyRes1 = null;

            IExecutionResult r2   = null;
            Func<int>        map2 = null;

            "Given: I have success result with value 2"
                .x( () => r1 = ExecutionResult.Success( 2 ) );
            "And I have map function x => x + 3"
                .x( () => map1 = x => x + 3 );
            "When I apply map function to success result"
                .x( () => applyRes1 = r1.Map( map1 ) );
            "Then I should get success result with value = 5"
                .x( () =>
                {
                    Assert.True( applyRes1.IsSuccess );
                    Assert.Equal( 5, applyRes1.SuccessValue );
                } );

            "I have success result"
                .x( () => r2 = ExecutionResult.Success() );
            "And I have map function () => 3"
                .x( () => map2 = () => 3 );

            "When I apply map function to success result"
                .x( () => applyRes1 = r2.Map( map2 ) );

            "Then I should get success result with value = 3"
                .x( () =>
                {
                    Assert.True( applyRes1.IsSuccess );
                    Assert.Equal( 3, applyRes1.SuccessValue );
                } );
        }
    }
}