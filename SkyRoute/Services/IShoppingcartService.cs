using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public interface IShoppingcartService
    {
        ShoppingCartVM GetShoppingCart(ISession session);
        void SetShoppingObject(ShoppingCartVM shoppingCartVM,ISession session);
        void UpdatePassengerShoppingCart(PassengerListVM model, ISession session);
    }
}