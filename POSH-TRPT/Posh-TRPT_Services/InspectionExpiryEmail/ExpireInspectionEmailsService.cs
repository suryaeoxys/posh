using AutoMapper;
using Posh_TRPT_Domain.Employees;
using Posh_TRPT_Domain.InspectionData;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.InspectionExpiryEmail
{
    public class ExpireInspectionEmailsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExpireInspectionEmails _inspectionRepository;
        public readonly IMapper _mapper;
        public ExpireInspectionEmailsService(IUnitOfWork unitOfWork
            , IExpireInspectionEmails inspectionRepository
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _inspectionRepository = inspectionRepository;
            _mapper = mapper;
        }
        public async Task<APIResponse<bool>> SendEmailForInspection()
        {
            APIResponse<bool> _APIResponse = new APIResponse<bool>();
            try
            {
               
                var userData = await _inspectionRepository.SendEmailToDriverAndADMIN();
                if (userData)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = true ;
                    _APIResponse.Message = "Sent Email";
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.AddFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.AddFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;
                throw;
            }
            return _APIResponse;
        }
    }
}
