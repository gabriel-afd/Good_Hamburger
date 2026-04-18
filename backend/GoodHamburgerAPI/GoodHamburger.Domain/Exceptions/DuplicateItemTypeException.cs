using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Exceptions
{
    public class DuplicateItemTypeException : DomainException
    {
        public DuplicateItemTypeException(ItemType type) 
            : base($"O pedido já contém um item do tipo '{type}'. Cada pedido pode ter apenas um sanduíche, uma batata e um refrigerante."){}
    }
}
