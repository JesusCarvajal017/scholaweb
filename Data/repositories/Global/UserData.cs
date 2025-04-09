using Data.interfaces;
using Entity;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.repositories.Global
{
    public class UserData : GenericData<User>
    {
        public UserData(ApplicationDbContext context, ILogger<User> logger) : base(context, logger)
        {

        }

    }
}