using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Posh_TRPT_Domain.PushNotification.GoogleNotification;

namespace Posh_TRPT_Domain.PushNotification
{
    public class NotificationModel
    {
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }
        [JsonPropertyName("isAndroidDevice")]
        public bool IsAndroidDevice { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
    public class Data
    {

        [JsonPropertyName("body")]
        public string? Body { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("key_1")]
        public UserData? UserData { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }
        [JsonPropertyName("data")]
        public Data? Data { get; set; }
        [JsonPropertyName("notification")]
        public Notification? Notification { get; set; }

    }

    public class Notification
    {

        [JsonPropertyName("body")]
        public string? Body { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("message")]
        public Message? Message { get; set; }
    }
}
