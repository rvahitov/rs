using System.Collections.Generic;

namespace Common.Specifications
{
    public interface ISpecificationResult 
    {
        bool IsSuccess { get; }
        IEnumerable<string> Errors { get; }
    }
}