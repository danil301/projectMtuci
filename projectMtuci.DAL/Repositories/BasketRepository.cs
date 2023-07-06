using Microsoft.EntityFrameworkCore;
using projectMtuci.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectMtuci.DAL.Repositories
{
    public class BasketRepository
    {
        private readonly ApplicationDbContext _db;

        private readonly SubjectRepository _subjectRepository;

        public List<Subject> Select(int userId)
        {
            List<Subject> subjects = new List<Subject>();
            foreach (var item in _db.Basket)
            {
                if (item.UserId == userId)
                {
                    subjects.Add(_db.Subjects.FirstOrDefault(x => x.Id == item.SubjectId));
                }
            }
            return subjects;
        }
    }
}
