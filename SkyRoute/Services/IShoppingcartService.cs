using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public interface IShoppingcartService
    {
        ShoppingCartVM GetShoppingCart(ISession session);
        void SetShoppingObject(ShoppingCartVM shoppingCartVM,ISession session);
        void SetSessionStart(ISession session,string sessionKey);
        void ClearSession(ISession session);
        void UpdatePassengerShoppingCart(PassengerListVM model, ISession session);
    }
}