using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Constants
{
    public static class ActivityActions
    {
        public const string Assign ="Assign";
        public const string CreateUser = "Create";
        public const string Block = "BLOCK";
        public const string GetAllUsers = "GetAllUssers";
        public const string GetUserActivity = "GetUserActivity";
        public const string GetUserByEmail = "GetUserByEmail";
        public const string GetUserById = "GetUserById";
        public const string GetUserRole = "GetUserRole";
        public const string RemoveRole = "RemoveRole";
        public const string RestoreUser = "RestoreUser";
        public const string SearchUsers = "SearchUsers";
        public const string SoftDeleteUser = "SoftDeleteUser";
        public const string UnblockUser = "UnblockUser";
        public const string UpdateRoles = "UpdateRoles";
        public const string UpdateUser = "UpdateUser";
        public const string UpdateUserStatus = "UpdateUserStatus";

    }
}
