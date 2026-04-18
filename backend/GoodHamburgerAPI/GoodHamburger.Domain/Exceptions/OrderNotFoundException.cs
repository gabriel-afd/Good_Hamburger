namespace GoodHamburger.Domain.Exceptions
{
    public class OrderNotFoundException : DomainException
    {
        public OrderNotFoundException(Guid orderId) : base($"Pedido com ID '{orderId}' não encontrado.") {}
    }
}
