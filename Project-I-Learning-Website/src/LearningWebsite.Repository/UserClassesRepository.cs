using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningWebsite.Repository
{
    public interface IUserClassesRepository
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

    public class UserClassesRepository : IUserClassesRepository
    {
        public UserClassesModel Add(int userId, int classId)
        {
            var user = DatabaseAccessor.Instance.Users.FirstOrDefault(t => t.UserId == userId);
            var classToAdd = DatabaseAccessor.Instance.Classes.FirstOrDefault(t => t.ClassId == classId);
            user.Classes.Add(classToAdd);

            DatabaseAccessor.Instance.SaveChanges();

            return new UserClassesModel
            {
                UserId = userId,
                ClassId = classToAdd.ClassId,
                Name = classToAdd.ClassName,
                Description = classToAdd.ClassDescription,
                Price = classToAdd.ClassPrice
            };
        }

        public UserClassesModel[] GetAll(int userId)
        {
            var user = DatabaseAccessor.Instance.Users.FirstOrDefault(t => t.UserId == userId);
            var classes = user.Classes.ToList();
            UserClassesModel[] res = new UserClassesModel[classes.Count];
            for(int i = 0; i < res.Length; i++)
            {
                res[i] = new UserClassesModel
                {
                    UserId = userId,
                    ClassId = classes.ElementAt(i).ClassId,
                    Name = classes.ElementAt(i).ClassName,
                    Description = classes.ElementAt(i).ClassDescription,
                    Price = classes.ElementAt(i).ClassPrice
                };
            }
                
            return res;
        }
    }
}
