using FinalProject.BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface IUsmanBL
    {
        //registration asp identity
        Task<bool> RegisterAsync(RegistrationDTO registrationDTO);
        //login asp identity
        Task<UserWithTokenDTO> LoginAsync(LoginDTO loginDTO);
        //create role
        Task<bool> CreateRoleAsync(RoleCreateDTO roleCreateDTO);
        //add user to role
        Task<bool> AddUserToRoleAsync(string email, string roleName);

        //get roles by user
        Task<List<string>> GetRolesByUserAsync(string email);
    }
}