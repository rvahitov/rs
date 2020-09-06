using System;
using Common.ExecutionResults;

namespace Common.Specifications
{
    public interface ISpecification<T>
    {
        IExecutionResult Check(T target);

        ISpecification<T> And(ISpecification<T> other)
        {
            IExecutionResult SpecFunc(T target)
            {
                var thisResult = Check(target);
                if (thisResult.IsSuccess) return other.Check(target);
                return thisResult;
            }

            return Specification.Create<T>(SpecFunc);
        }

        ISpecification<T> Or(ISpecification<T> other)
        {
            IExecutionResult SpecFunc(T target)
            {
                var result = Check(target);
                if (result.IsSuccess) return result;
                return other.Check(target);
            }

            return Specification.Create<T>(SpecFunc);
        }

        public static ISpecification<T> operator &(ISpecification<T> left, ISpecification<T> right) => left.And(right);

        public static ISpecification<T> operator |(ISpecification<T> left, ISpecification<T> right) => left.Or(right);
    }

    public static class Specification
    {
        public static ISpecification<T> Create<T>(Func<T, IExecutionResult> specFunc) => new Spec<T>(specFunc);

        public static ISpecification<T> Create<T>(Func<T, bool> predicate, string falseMessage)
        {
            IExecutionResult SpecFunc(T target)
            {
                if (predicate(target)) return Success();
                return Failed(falseMessage);
            }

            return Create<T>(SpecFunc);
        }

        public static IExecutionResult Success() => ExecutionResult.Success();

        public static IExecutionResult Failed(params string[] errors) => ExecutionResult.Failure(errors);

        private sealed class Spec<T> : ISpecification<T>
        {
            private readonly Func<T, IExecutionResult> _specFunction;

            public Spec(Func<T, IExecutionResult> specFunction)
            {
                _specFunction = specFunction;
            }

            public IExecutionResult Check(T target) => _specFunction.Invoke(target);
        }
    }
}