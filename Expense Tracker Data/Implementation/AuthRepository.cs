using AutoMapper;
using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Context;
using Expense_Tracker_Data.Interface;
using Expense_Tracker_Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_Data.Implementation
{
    public class AuthRepository : IAuth
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserRes> AuthenticateUser(LoginCred cred)
        {
            var user = await _context.Users.Where(u => (cred.Username == u.Email || cred.Username == u.Phone) && cred.Password == u.Password).FirstOrDefaultAsync();
            if(user == null)
            {
                return null;
            }
            var _user = _mapper.Map<UserRes>(user);
            return _user;
        }

        public Task<bool> Singup()
        {
            throw new NotImplementedException();
        }
    }
}
