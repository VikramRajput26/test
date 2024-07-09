using Microsoft.AspNetCore.Mvc;
using StudentMVC.Models;
using StudentMVC.Services;
using System;

namespace StudentMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentServices _studentService;

        public StudentController(IStudentServices studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(Student model)
        {
            if (ModelState.IsValid)
            {
                _studentService.Insert(model);
                return RedirectToAction("GetStudents");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var products = _studentService.GetAll();
            return View(products);
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _studentService.Delete(id);
            return RedirectToAction("GetStudents");
        }

        public IActionResult GetById()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetById(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpGet]
        public IActionResult Update()
        {
            return View(new Student());
        }

        [HttpPost]
        public IActionResult Update(Student model)
        {
            if (ModelState.IsValid)
            {
                _studentService.Update(model);
                return RedirectToAction("GetStudents");
            }
            return View(model);
        }

        // Admin login method
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(AdminLoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "abc" && model.Password == "123")
                {
                    return RedirectToAction("Dashboard");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult StudentLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StudentLogin(StudentLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var student = _studentService.GetAll().FirstOrDefault(s => s.email == model.Username && s.mobileNumber == model.Password);
                if (student != null)
                {
                    return RedirectToAction("GetById", new { id = student.id });
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditStudentDetails(int id)
        {
            var student = _studentService.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudentDetails(Student model)
        {
            if (ModelState.IsValid)
            {
                var existingStudent = _studentService.GetById(model.id);
                if (existingStudent != null)
                {
                    existingStudent.name = model.name;
                    existingStudent.email = model.email;
                    existingStudent.mobileNumber = model.mobileNumber;
                    existingStudent.address = model.address;
                    existingStudent.status = model.status;

                    _studentService.Update(existingStudent);
                    return RedirectToAction("GetById", new { id = model.id });
                }
            }
            return View(model);
        }

        public IActionResult SearchByStatus()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchByStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                ModelState.AddModelError("", "Status cannot be empty.");
                return View();
            }

            var students = _studentService.GetAll()
                              .Where(s => s.status.Equals(status, StringComparison.OrdinalIgnoreCase))
                              .ToList();
            return View("DisplayStudents", students);
        }
        [HttpGet]
        public IActionResult SortByStatus()
        {
            var sortedStudents = _studentService.GetAll().OrderBy(s => s.status).ToList();
            return View("DisplayStudents", sortedStudents);
        }
    }
}
