using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace Domain.SpecFlow.Steps
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private readonly List<int> _numbers = new List<int>();
        private int _result;

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            _numbers.Add(number);
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            _numbers.Add(number);
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            _result = _numbers.Sum();
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            Assert.Equal(result, _result);
        }
    }
}