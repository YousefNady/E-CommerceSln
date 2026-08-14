namespace E_Commerce.Services.Exceptions
{
    // that class is base for any not found exception in the project
    public abstract class NotFoundException(string Message) : Exception(Message) // from c# 12 primary constructor || one of features of primary constructor is readonly properties Immutable
    {

    }

    public sealed class ProductNotFoundException(int id) : NotFoundException($"Product with ID: {id} is not found")
    {
        
    }

    public sealed class BasketNotFoundException(string id) : NotFoundException($"Basket with ID: {id} is not found")
    {

    }
}
