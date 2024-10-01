using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
   
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class CardNew
        {
            [JsonProperty("reference")]
            public string? Reference { get; set; }

            [JsonProperty("reference_status")]
            public string? ReferenceStatus { get; set; }

            [JsonProperty("reference_type")]
            public string? ReferenceType { get; set; }

            [JsonProperty("type")]
            public string? Type { get; set; }
        }

        public class DestinationDetails
        {
            [JsonProperty("card")]
            public CardNew? Card { get; set; }

            [JsonProperty("type")]
            public string? Type { get; set; }
        }


        public class StripePaymentReturnModel
    {
            [JsonProperty("id")]
            public string? Id { get; set; }

            [JsonProperty("object")]
            public string? Object { get; set; }

            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("balance_transaction")]
            public string? BalanceTransaction { get; set; }

            [JsonProperty("charge")]
            public string? Charge { get; set; }

            [JsonProperty("created")]
            public int Created { get; set; }

            [JsonProperty("currency")]
            public string? Currency { get; set; }

            [JsonProperty("destination_details")]
            public DestinationDetails? DestinationDetails { get; set; }

            [JsonProperty("metadata")]
            public Metadata? Metadata { get; set; }

            [JsonProperty("payment_intent")]
            public string? PaymentIntent { get; set; }

            [JsonProperty("reason")]
            public object? Reason { get; set; }

            [JsonProperty("receipt_number")]
            public object? ReceiptNumber { get; set; }

            [JsonProperty("source_transfer_reversal")]
            public object? SourceTransferReversal { get; set; }

            [JsonProperty("status")]
            public string? Status { get; set; }

            [JsonProperty("transfer_reversal")]
            public object? TransferReversal { get; set; }
        

    }
}
