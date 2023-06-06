namespace Juspay
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class Refund : JuspayResponse
    {
        public string Id
        {
            get { return GetValue<string>("id"); }
            set { SetValue("id", value); }
        }

        public string UniqueRequestId
        {
            get { return GetValue<string>("unique_request_id"); }
            set { SetValue("unique_request_id", value); }
        }

        public string Ref
        {
            get { return GetValue<string>("ref"); }
            set { SetValue("ref", value); }
        }

        public double Amount
        {
            get { return GetValue<double>("amount"); }
            set { SetValue("amount", value); }
        }

        public string Created
        {
            get { return GetValue<string>("created"); }
            set { SetValue("created", value); }
        }

        public string Status
        {
            get { return GetValue<string>("status"); }
            set { SetValue("status", value); }
        }

        public string ErrorMessage
        {
            get { return GetValue<string>("error_message"); }
            set { SetValue("error_message", value); }
        }

        public string InitiatedBy
        {
            get { return GetValue<string>("initiated_by"); }
            set { SetValue("initiated_by", value); }
        }

        public bool SentToGateway
        {
            get { return GetValue<bool>("sent_to_gateway"); }
            set { SetValue("sent_to_gateway", value); }
        }

        public string Arn
        {
            get { return GetValue<string>("arn"); }
            set { SetValue("arn", value); }
        }

        public string InternalReferenceId
        {
            get { return GetValue<string>("internal_reference_id"); }
            set { SetValue("internal_reference_id", value); }
        }

        public string Gateway
        {
            get { return GetValue<string>("gateway"); }
            set { SetValue("gateway", value); }
        }

        public string EpgTxnId
        {
            get { return GetValue<string>("epg_txn_id"); }
            set { SetValue("epg_txn_id", value); }
        }

        public string AuthorizationId
        {
            get { return GetValue<string>("authorization_id"); }
            set { SetValue("authorization_id", value); }
        }

        public string ReferenceId
        {
            get { return GetValue<string>("reference_id"); }
            set { SetValue("reference_id", value); }
        }

        public string ResponseCode
        {
            get { return GetValue<string>("response_code"); }
            set { SetValue("response_code", value); }
        }

        public string RefundArn
        {
            get { return GetValue<string>("refund_arn"); }
            set { SetValue("refund_arn", value); }
        }

        public string RefundType
        {
            get { return GetValue<string>("refund_type"); }
            set { SetValue("refund_type", value); }
        }

        public string RefundSource
        {
            get { return GetValue<string>("refund_source"); }
            set { SetValue("refund_source", value); }
        }

        public string TxnId
        {
            get { return GetValue<string>("txn_id"); }
            set { SetValue("txn_id", value); }
        }

        public string OrderId
        {
            get { return GetValue<string>("order_id"); }
            set { SetValue("order_id", value); }
        }

        public Dictionary<string, object> Metadata
        {
            get { return GetValue<Dictionary<string, object>>("metadata"); }
            set { SetValue("metadata", value); }
        }

    }
}