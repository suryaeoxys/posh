using AutoMapper;
using Posh_TRPT_Domain.Employees;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Models.DTO;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Employees
{
    public  class EmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _userRepository;
        public readonly IMapper _mapper;
        public EmployeeService(IUnitOfWork unitOfWork
            , IEmployeeRepository userRepository
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<EmployeeDTO>> AddUser(EmployeeDTO user)
        {

            APIResponse<EmployeeDTO> _APIResponse = new APIResponse<EmployeeDTO>();

            try
            {
                var userData = _mapper.Map<Employee>(user);

                _userRepository.Add(userData);

                if (await _unitOfWork.CommitAsync() > 0)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = user;
                    _APIResponse.Message = EmployeeResource.AddSuccess;
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

        public async Task<APIResponse<IEnumerable<EmployeeDTO>>> GetUsers()
        {
            APIResponse<IEnumerable<EmployeeDTO>> _APIResponse = new APIResponse<IEnumerable<EmployeeDTO>>();

            try
            {
                var users = await _userRepository.GetUsers();

                if (users != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<EmployeeDTO>>(users);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }


            return _APIResponse;
        }

        public async Task<APIResponse<IEnumerable<EmployeeDTO>>> GetByUsername(string username)
        {
            APIResponse<IEnumerable<EmployeeDTO>> _APIResponse = new APIResponse<IEnumerable<EmployeeDTO>>();

            try
            {
                var result = await _userRepository.GetByUsername(username);

                if (result != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<EmployeeDTO>>(result);
                    _APIResponse.Message = EmployeeResource.FetchSuccess;
                    _APIResponse.Status = HttpStatusCode.OK;
                }
                else
                {
                    _APIResponse.Success = false;
                    _APIResponse.Message = EmployeeResource.FetchFailed;
                    _APIResponse.Status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                _APIResponse.Success = false;
                _APIResponse.Error = new CustomException(ex.Message, ex.InnerException);
                _APIResponse.Message = EmployeeResource.FetchFailed;
                _APIResponse.Status = HttpStatusCode.InternalServerError;

                throw;
            }

            return _APIResponse;
        }
    }
}
