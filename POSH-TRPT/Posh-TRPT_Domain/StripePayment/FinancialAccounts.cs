using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
   
        public class AccountHolder
        {
            [JsonProperty("customer")]
            public string? Customer { get; set; }

            [JsonProperty("type")]
            public string? Type { get; set; }
        }

        public class FinancialAccounts
        {
            [JsonProperty("id")]
            public string? Id { get; set; }

            [JsonProperty("object")]
            public string? Object { get; set; }

            [JsonProperty("account_holder")]
            public AccountHolder? AccountHolder { get; set; }

            [JsonProperty("balance")]
            public object? Balance { get; set; }

            [JsonProperty("balance_refresh")]
            public object? BalanceRefresh { get; set; }

            [JsonProperty("category")]
            public string? Category { get; set; }

            [JsonProperty("created")]
            public int Created { get; set; }

            [JsonProperty("display_name")]
            public string? DisplayName { get; set; }

            [JsonProperty("institution_name")]
            public string? InstitutionName { get; set; }

            [JsonProperty("last4")]
            public string? Last4 { get; set; }

            [JsonProperty("livemode")]
            public bool Livemode { get; set; }

            [JsonProperty("ownership")]
            public object? Ownership { get; set; }

            [JsonProperty("ownership_refresh")]
            public object? OwnershipRefresh { get; set; }

            [JsonProperty("permissions")]
            public List<object>? Permissions { get; set; }

            [JsonProperty("status")]
            public string? Status { get; set; }

            [JsonProperty("subcategory")]
            public string? Subcategory { get; set; }

            [JsonProperty("subscriptions")]
            public List<object>? Subscriptions { get; set; }

            [JsonProperty("supported_payment_method_types")]
            public List<string>? SupportedPaymentMethodTypes { get; set; }

            [JsonProperty("transaction_refresh")]
            public object? TransactionRefresh { get; set; }
        }
    
}
