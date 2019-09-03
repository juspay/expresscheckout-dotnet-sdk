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
            string nameOnCard=null, string nickname=null, ECApiCredentials credentials = null)
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

            return await HTTPUtils.ParseAndWrapResponseJObject(await HTTPUtils.DoPost("/cards", req, credentials));
        }

        // Given a customerId, list all the cards for that customer
        private static async Task<ECApiResponse> List(string customerId, ECApiCredentials credentials = null)
        {
            if(string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentException("customerId is not specified, please specify a customer"
                    + " for which we need to fetch cards");
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(await HTTPUtils.DoGet("/cards", new Dictionary<string, string>()
            {
                { "customer_id", customerId }
            }, credentials));
        }

        // Given a card reference, delete the card
        private static async Task<ECApiResponse> Delete(string cardRef, ECApiCredentials credentials = null)
        {
            if(string.IsNullOrEmpty(cardRef))
            {
                throw new ArgumentException("Please specify a cardReference to the card that you want to delete");
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(await HTTPUtils.DoDelete(
                String.Format("/cards/{0}", cardRef),
                null,
                credentials
            ));
        }
    }
}
