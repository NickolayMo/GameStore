using GameStore.WebUI.Models.Entities;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.WebUI.Ifrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string _sessionKey = "Cart";
        public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
        {
            if(bindingContext.ModelType == typeof(Cart))
            {
                Cart cart = null;
               if(bindingContext.OperationBindingContext.HttpContext.Session != null)
                {
                    cart = GetCart(bindingContext.OperationBindingContext.HttpContext.Session);
                }
               return Task.FromResult(ModelBindingResult.Success(bindingContext.ModelName, cart));

            }     

            return Task.FromResult(ModelBindingResult.NoResult);
        }

        public static Cart GetCart(ISession session)
        {
            byte[] cartStrored = session.Get(_sessionKey);
            if (cartStrored != null)
            {
                string json = System.Text.Encoding.UTF8.GetString(cartStrored);
                return JsonConvert.DeserializeObject<Cart>(json);
            }
            Cart cart = new Cart();
            SetUpCart(cart, session);
            return cart;
        }
        public static void SetUpCart(Cart cart, ISession session)
        {
            string objJsone = JsonConvert.SerializeObject(cart);
            byte[] serializedResult = System.Text.Encoding.UTF8.GetBytes(objJsone);
            session.Set(_sessionKey, serializedResult);
        }
    }
}
