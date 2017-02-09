using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Juspay.ExpressCheckout.Base;

namespace Juspay.ExpressCheckout
{
    // All Card related API's are here 
    public class StoredCards
    {
        public static async Task<ECApiResponse> AddCardUsingToken(string cardToken, string customerId, string customerEmail,
            string nameOnCard=null, string nickname=null)
        {
            if(cardToken == null || customerEmail == null || customerId == null)
            {
                throw new ArgumentException("cardToken, customerEmail and customerId are required parameters");
            }

            var req = new Dictionary<string, string>()
            {
                { "card_token", cardToken },
                { "customer_id", customerId },
                { "customer_email", customerEmail },
                { "merchant_id", Config.GetMerchantId() }
            };

            if(nameOnCard != null)
            {
                req.Add("name_on_card", nameOnCard);
            }

            if(nickname == null)
            {
                req.Add("nickname", nickname);
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(await HTTPUtils.DoPost("/cards", req));
        }


        private static async Task<ECApiResponse> List(string customerId)
        {
            
        }
    }
}
