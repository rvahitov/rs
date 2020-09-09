using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ExecutionResults
{
    public interface IExecutionResult
    {
        bool                IsSuccess { get; }
        IEnumerable<string> Errors    { get; }
    }

    public interface IExecutionResult<out T> : IExecutionResult
    {
        T SuccessValue { get; }
    }

    public static class ExecutionResult
    {
        public static IExecutionResult Success() => new Result( Enumerable.Empty<string>() );

        public static IExecutionResult<T> Success<T>( T value ) => new Result<T>( value );

        public static IExecutionResult Failed( params string[] errors )
        {
            if ( errors        == null ) throw new ArgumentNullException( nameof( errors ) );
            if ( errors.Length == 0 ) throw new ArgumentException( "Value cannot be an empty collection.", nameof( errors ) );
            return new Result( errors );
        }

        public static IExecutionResult<TResult> Map<T, TResult>( this IExecutionResult<T> r, Func<T, TResult> mapper ) =>
            r.IsSuccess ? Success( mapper( r.SuccessValue ) ) : Failed<TResult>( r.Errors.ToArray() );

        public static IExecutionResult<TResult> Map<TResult>( this IExecutionResult r, Func<TResult> mapper ) =>
            r.IsSuccess ? Success( mapper() ) : Failed<TResult>( r.Errors.ToArray() );

        public static IExecutionResult<T> Failed<T>( params string[] errors )
        {
            if ( errors        == null ) throw new ArgumentNullException( nameof( errors ) );
            if ( errors.Length == 0 ) throw new ArgumentException( "Value cannot be an empty collection.", nameof( errors ) );
            return new Result<T>( errors );
        }

        private class Result : IExecutionResult
        {
            private readonly string[] _errors;

            public Result( IEnumerable<string> errors )
            {
                _errors   = errors.ToArray();
                IsSuccess = _errors.Length == 0;
            }

            public bool                IsSuccess { get; }
            public IEnumerable<string> Errors    => _errors;
        }

        private sealed class Result<T> : Result, IExecutionResult<T>
        {
            public Result( IEnumerable<string> errors ) : base( errors )
            {
                SuccessValue = default;
            }

            public Result( T successValue ) : base( Enumerable.Empty<string>() )
            {
                SuccessValue = successValue;
            }

            public T SuccessValue { get; }
        }
    }
}