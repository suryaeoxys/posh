using AutoMapper;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.PushNotification;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.PushNotificationDTO;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.PushNotification
{
    public class PushNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPushNotificationRepository _pushNotificationRepository;
        public readonly IMapper _mapper;
        public PushNotificationService(IUnitOfWork unitOfWork
            , IPushNotificationRepository pushNotificationRepository
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _pushNotificationRepository = pushNotificationRepository;
            _mapper = mapper;
        }

		#region SendNotification
		/// <summary>
		/// method to SendNotification
		/// </summary>
		/// <param name="notification"></param>
		/// <returns></returns>
		public async Task<APIResponse<bool>> SendNotification(NotificationModelDTO notification)
        {
            APIResponse<bool> _APIResponse = new APIResponse<bool>();

            try
            {
                var newNotification = _mapper.Map<NotificationModel>(notification);
                var notification1 = await _pushNotificationRepository.SendNotification(newNotification);

                if (notification1)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<bool>(true);
                    _APIResponse.Message = GlobalResourceFile.PushNotification;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = GlobalResourceFile.PushNotificationFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = GlobalResourceFile.Exception;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        } 
        #endregion
    }
}
