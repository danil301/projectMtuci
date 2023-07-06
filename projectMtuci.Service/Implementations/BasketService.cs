using projectMtuci.DAL.Repositories;
using projectMtuci.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMtuci.Service.Implementations
{
    public class BasketService
    {
        private readonly BasketRepository _basketRepository;

        public List<Subject> GetSubjectsForUser(int userId)
        {
            return _basketRepository.Select(userId);
        }
    }
}
