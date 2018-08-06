using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Juspay.ExpressCheckout.Base;

namespace Juspay.ExpressCheckout
{
    // All Customer related API's are here 
    public class Customer
    {
        public static async Task<ECApiResponse> CreateCustomer(string objectReferenceId, 
                                                                  string mobileNumber,
                                                                  string emailAddress,
                                                                  string firstName=null,
                                                                  string lastName=null,
                                                                  string mobileCountryCode=null)
        {
            if(objectReferenceId == null || mobileNumber == null || emailAddress == null)
            {
                throw new ArgumentException("objectReferenceId, mobileNumber and emailAddress are required parameters");
            }

            var req = new Dictionary<string, string>()
            {
                { "object_reference_id", objectReferenceId },
                { "mobile_number", mobileNumber },
                { "email_address", emailAddress }
            };

            if(firstName != null) 
            {
                req.Add("first_name", firstName);
            }

            if(lastName != null)
            {
                req.Add("last_name", lastName);
            }

            if(mobileCountryCode != null)
            {
                req.Add("mobile_country_code", mobileCountryCode);
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(await HTTPUtils.DoPost("/customers", req));
        }

        public static async Task<ECApiResponse> GetCustomer(string customerId)
        {
            if(string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentException("customerId is not specified, please specify a customerId");
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(await HTTPUtils.DoGet(
                String.format("/customers/{0}", customerId)));
        }

        // Given a customerId, list all the cards for that customer
        public static async Task<ECApiResponse> Update(string customerId)
        {
            if(string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentException("customerId is not specified, please specify a customer"
                    + " for which we need to fetch cards");
            }

            return await HTTPUtils.ParseAndWrapResponseJObject(await HTTPUtils.DoGet("/cards", new Dictionary<string, string>()
            {
                { "customer_id", customerId }
            }));
        }
    }
}
