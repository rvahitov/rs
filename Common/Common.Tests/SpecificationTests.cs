using Common.Specifications;
using Xunit;

namespace Common.Tests
{
    public class SpecificationTests
    {
        [Fact]
        public void TestCheckSpecification()
        {
            var spec = Specification.Create<int>(i =>
            {
                if (i >= 0) return Specification.Success();
                return Specification.Failed("NUmber is negative");
            });
            var result = spec.Check(1);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);

            result = spec.Check(-2);
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void TestSpecificationAnd()
        {
            var spec1 = Specification.Create<int>(i => i > 0, "Number is negative");
            var spec2 = Specification.Create<int>(i => i % 2 == 0, "Number is odd");
            var spec3 = spec1 & spec2;

            var result = spec3.Check(-1);
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Errors);

            result = spec3.Check(1);
            Assert.NotEmpty(result.Errors);

            result = spec1.Check(2);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void TestSpecificationOr()
        {
            var spec1 = Specification.Create<string>(s => s == "Fizz", "Invalid string");
            var spec2 = Specification.Create<string>(s => s == "Buzz", "Invalid string");
            var spec3 = spec1 | spec2;

            var result = spec3.Check("Fizz");
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);
            result = spec3.Check("Buzz");
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);
            result = spec3.Check("FizzBuzz");
            Assert.False(result.IsSuccess);
            Assert.NotEmpty(result.Errors);
        }
    }
}