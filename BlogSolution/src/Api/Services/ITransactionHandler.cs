namespace Api.Services;

public interface ITransactionHandler
{
    Task<T> PerformTransaction<T>(Func<Task<T>> func);
}