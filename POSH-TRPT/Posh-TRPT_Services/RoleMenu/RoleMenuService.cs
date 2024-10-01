using AutoMapper;
using Posh_TRPT_Domain.Interfaces;
using Posh_TRPT_Domain.RoleMenu;
using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Models.DTO.RoleMenuDTO;
using Posh_TRPT_Utility.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.RoleMenu
{
    public class RoleMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleMenuRepository _menuRepository;
        public readonly IMapper _mapper;
        public RoleMenuService(IUnitOfWork unitOfWork
            , IRoleMenuRepository menuRepository
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }


        public async Task<APIResponse<IEnumerable<RoleMenuDTO>>> GetMenuMaster()
        {
            APIResponse<IEnumerable<RoleMenuDTO>> _APIResponse = new APIResponse<IEnumerable<RoleMenuDTO>>();

            try
            {
                var users = await _menuRepository.GetMenuMaster();

                if (users != null)
                {
                    _APIResponse.Success = true;
                    _APIResponse.Data = _mapper.Map<IEnumerable<RoleMenuDTO>>(users);
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

        public async Task<APIResponse<IEnumerable<RoleMenuDTO>>> GetMenuMaster(string UserRole)
        {
            APIResponse<IEnumerable<RoleMenuDTO>> _APIResponse = new APIResponse<IEnumerable<RoleMenuDTO>>();

            try
            {
                var users = await _menuRepository.GetMenuMaster(UserRole);

                if (users != null)
                {
                    _APIResponse.Success = true;
                    //  _APIResponse.Data = //_mapper.Map<IEnumerable<EmployeeDTO>>(users);
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
