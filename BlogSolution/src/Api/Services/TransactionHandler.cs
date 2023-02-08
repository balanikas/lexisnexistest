namespace Api.Services;

internal class TransactionHandler : ITransactionHandler
{
    private readonly IDatabaseContext _context;


    public TransactionHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<T> PerformTransaction<T>(Func<Task<T>> func)
    {
        using var session = await _context.Client.StartSessionAsync();
        session.StartTransaction();

        T returnedValue;
        try
        {
            returnedValue = await func();
        }
        catch
        {
            await session.AbortTransactionAsync();
            throw;
        }

        await session.CommitTransactionAsync();
        return returnedValue;
    }
}