using CorePush.Apple;
using CorePush.Google;
using Google.Apis.Auth.OAuth2;
using iTextSharp.text.log;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Posh_TRPT_Domain.PushNotification;
using Posh_TRPT_Utility.JwtUtils;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Posh_TRPT_Domain.PushNotification.GoogleNotification;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class PushNotificationRepository : Repository<NotificationModel>, IPushNotificationRepository
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;
        private readonly ILogger<PushNotificationRepository> _logger;
        public PushNotificationRepository(DbFactory dbFactory, IWebHostEnvironment _environment, IConfiguration _config, ILogger<PushNotificationRepository> _logger) : base(dbFactory)
        {
            this._environment = _environment;
            this._config = _config;
            this._logger = _logger;
        }

        public async Task<bool> SendNotification(NotificationModel notificationModel)
        {
            try
            {
                if(notificationModel.IsAndroidDevice)
                {
                    _logger.LogInformation("{0} InSide  SendNotification in BookingSystemRepository Method :  DriverDeviceId = {1} ", DateTime.UtcNow, notificationModel?.DeviceId);
                    string fileName = Path.Combine(_environment.WebRootPath, "helpful-cosine-410722-firebase-adminsdk-yftwt-fbcdb2d817.json"); //Download from Firebase Console ServiceAccount
                    string scopes = _config["GoogleNotification:Scopes"]!;
                    var bearertoken = ""; // Bearer Token in this variable
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        bearertoken = GoogleCredential
                          .FromStream(stream) // Loads key file
                          .CreateScoped(scopes) // Gathers scopes requested
                          .UnderlyingCredential // Gets the credentials
                          .GetAccessTokenForRequestAsync().Result; // Gets the Access Token

                    }
                    ///--------Calling FCM-----------------------------

                    var clientHandler = new HttpClientHandler();
                    var client = new HttpClient(clientHandler);

                    client.BaseAddress = new Uri(_config["GoogleNotification:BaseAddress"]!); // FCM HttpV1 API

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Configuration.Content));

                    //client.DefaultRequestHeaders.Accept.Add("Authorization", "Bearer " + bearertoken);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtAuthenticationDefaults.BearerPrefix, bearertoken); // Authorization Token in this variable

                    //---------------Assigning Of data To Model --------------

                    Root rootObj = new Root();
                    rootObj.Message = new Posh_TRPT_Domain.PushNotification.Message();
                    rootObj.Message.Token = notificationModel!.DeviceId; //FCM Token id
                    rootObj.Message.Data = new Posh_TRPT_Domain.PushNotification.Data();
                    rootObj.Message.Data.Title = notificationModel.Title;
                    rootObj.Message.Data.Body = notificationModel.Body;
                    //rootObj.Message.Data.UserData = userData;
                    rootObj.Message.Notification = new Notification();
                    rootObj.Message.Notification.Title = notificationModel.Title;
                    rootObj.Message.Notification.Body = notificationModel.Body;

                    //-------------Convert Model To JSON ----------------------

                    var jsonObj = System.Text.Json.JsonSerializer.Serialize<Object>(rootObj);

                    //------------------------Calling Of FCM Notify API-------------------

                    var data = new StringContent(jsonObj, Encoding.UTF8, Configuration.Content);
                    data.Headers.ContentType = new MediaTypeHeaderValue(Configuration.Content);

                    var response = client.PostAsync(_config["GoogleNotification:BaseAddress"]!, data).Result; // Calling The FCM httpv1 API

                    //---------- Deserialize Json Response from API ----------------------------------

                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var responseObj = System.Text.Json.JsonSerializer.Serialize<object>(jsonResponse);
                    _logger.LogInformation("{0} Inside After send notification getting response : {1}", DateTime.UtcNow, jsonResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("{0} InSide  SendNotification in BookingSystemRepository Method -- Success : ==> {1}", DateTime.UtcNow, response.IsSuccessStatusCode);
                        return true;
                    }
                    //return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} InSide  SendNotification in BookingSystemRepository Method -- Notification_Error : {1}", DateTime.UtcNow, ex.Message);
                throw;
            }
        }
    }
}
