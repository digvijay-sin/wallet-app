using Expense_Tracker_Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Data.Interface
{
    public interface IAuth
    {
        Task<UserRes> AuthenticateUser(LoginCred cred);
        Task<bool> Singup();
    }
}
