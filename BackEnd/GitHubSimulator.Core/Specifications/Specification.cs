using System.Linq.Expressions;

namespace GitHubSimulator.Core.Specifications;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> func = ToExpression().Compile();
        return func(entity);
    }
}
