using SkyRoute.Extensions;
using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public class ShoppingcartService : IShoppingcartService
    {


        public ShoppingCartVM GetShoppingCart(ISession session)
        {
            return session.GetObject<ShoppingCartVM>("ShoppingCart") ?? new ShoppingCartVM();
        }

        public void SetSessionStart(ISession session, string sessionKey)
        {
            session.SetString(sessionKey, DateTime.UtcNow.ToString());
        }

        public void SetShoppingObject(ShoppingCartVM shoppingCartVM, ISession session)
        {
            session.SetObject("ShoppingCart", shoppingCartVM);
        }
        public void ClearSession(ISession session)
        {
            session.Clear();
        }
        public void UpdatePassengerShoppingCart(PassengerListVM model, ISession session)
        {
            var cart = GetShoppingCart(session);
            cart.Passengers = model.Passengers;
            session.SetObject("ShoppingCart", cart);
        }
    }
}