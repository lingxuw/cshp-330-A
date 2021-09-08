using LearningWebsite.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningWebsite.Repository
{
    public class DatabaseAccessor
    {
        private static readonly CourseEntities entities;

        static DatabaseAccessor()
        {
            entities = new CourseEntities();
            entities.Database.Connection.Open();
        }

        public static CourseEntities Instance
        {
            get
            {
                return entities;
            }
        }
    }
}
