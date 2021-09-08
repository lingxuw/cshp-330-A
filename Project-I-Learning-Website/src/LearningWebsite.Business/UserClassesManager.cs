using LearningWebsite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningWebsite.Business
{
    public interface IUserClassesManager
    {
        UserClassesModel Add(int userId, int classId);
        UserClassesModel[] GetAll(int userId);
    }

    public class UserClassesModel
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class UserClassesManager : IUserClassesManager
    {
        private readonly IUserClassesRepository userClassesRepository;

        public UserClassesManager(IUserClassesRepository userClassesRepository)
        {
            this.userClassesRepository = userClassesRepository;
        }

        public UserClassesModel Add(int userId, int classId)
        {
            var item = userClassesRepository.Add(userId, classId);

            return new UserClassesModel
            {
                UserId = item.UserId,
                ClassId = item.ClassId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price
            };
        }

        public UserClassesModel[] GetAll(int userId)
        {
            var items = userClassesRepository.GetAll(userId)
                .Select(t => new UserClassesModel
                {
                    UserId = t.UserId,
                    ClassId = t.ClassId,
                    Name = t.Name,
                    Description = t.Description,
                    Price = t.Price
                })
                .ToArray();

            return items;
        }
    }
}
