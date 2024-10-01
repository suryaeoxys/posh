using Posh_TRPT_Utility.ConstantStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.LoginDTO
{
    public class LoginResponseDTO
    {
        [JsonPropertyName(JsonHeaderData.token)]
        public string? Token { get; set; }
        [JsonPropertyName(JsonHeaderData.refreshToken)]
        public string? RefreshToken { get; set; }
        [JsonPropertyName(JsonHeaderData.expiration)]
        public string? Expiration { get; set; }
        [JsonPropertyName(JsonHeaderData.name)]
        public string? Name { get; set; }
        [JsonPropertyName(JsonHeaderData.username)]
        public string? Username { get; set; }
        [JsonPropertyName(JsonHeaderData.role)]
        public string? Role { get; set; }
    }
}
