using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Specifications
{
    public interface ISpecification<T>
    {
        ISpecificationResult Check(T target);

        ISpecification<T> And(ISpecification<T> other)
        {
            ISpecificationResult SpecFunc(T target)
            {
                var thisResult = Check(target);
                if (!thisResult.IsSuccess) return thisResult;
                return other.Check(target);
            }

            return Specification.Create<T>(SpecFunc);
        }

        ISpecification<T> Or(ISpecification<T> other)
        {
            ISpecificationResult SpecFunc(T target)
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
        public static ISpecification<T> Create<T>(Func<T, ISpecificationResult> specFunc) => new Spec<T>(specFunc);

        public static ISpecification<T> Create<T>(Func<T, bool> predicate, string falseMessage)
        {
            ISpecificationResult SpecFunc(T target)
            {
                if (predicate(target)) return Success();
                return Failed(falseMessage);
            }

            return Create<T>(SpecFunc);
        }

        public static ISpecificationResult Success() => new SpecResult(Enumerable.Empty<string>());

        public static ISpecificationResult Failed(params string[] errors) => new SpecResult(errors);

        private sealed class Spec<T> : ISpecification<T>
        {
            private readonly Func<T, ISpecificationResult> _specFunction;

            public Spec(Func<T, ISpecificationResult> specFunction)
            {
                _specFunction = specFunction;
            }

            public ISpecificationResult Check(T target) => _specFunction.Invoke(target);
        }

        private sealed class SpecResult : ISpecificationResult
        {
            private readonly string[] _errors;

            public SpecResult(IEnumerable<string> errors)
            {
                _errors = errors.ToArray();
                IsSuccess = _errors.Length == 0;
            }

            public IEnumerable<string> Errors => _errors;
            public bool IsSuccess { get; }
        }
    }
}