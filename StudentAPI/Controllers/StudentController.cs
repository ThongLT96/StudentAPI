using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<Student>>> Get()
        {
            return Ok(await _context.Students.ToListAsync());
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return BadRequest("Không tìm thấy sinh viên nào có mã số " + id);

            return Ok(student);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<Student>> UpdateStudent(Student request)
        {
            var student = await _context.Students.FindAsync(request.Id);
            if (student == null)
                return BadRequest("Không tìm thấy sinh viên này có mã số " + request.Id);

            student.IDNumber = request.IDNumber;
            student.PhoneNumber = request.PhoneNumber;
            student.LastName = request.LastName;
            student.FirstName = request.FirstName;

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult >= 0)
                return Ok(await _context.Students.ToListAsync());
            return UnprocessableEntity("Lỗi");
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<List<Student>>> AddStudent(Student student)
        {
            _context.Students.Add(student);
            var saveResult = await _context.SaveChangesAsync();
            if (saveResult >= 0)
                return Ok(await _context.Students.ToListAsync());
            return UnprocessableEntity("Lỗi");
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> RemoveStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return BadRequest("Không tìm thấy sinh viên có mã số " + id);

            _context.Students.Remove(student);
            var saveResult = await _context.SaveChangesAsync();
            if (saveResult >= 0)
                return Ok();
            return UnprocessableEntity("Lỗi");
        }
    }
}
