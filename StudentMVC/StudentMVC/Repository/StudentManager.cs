using StudentMVC.Models;

namespace StudentMVC.Repository
{
    public class StudentManager
    {
        public static List<Student> getAllStudents()
        {
            using (var context = new StControllerContext())
            {
                var students = from m in context.Students select m;
                return students.ToList();
            }
        }
    }
}
