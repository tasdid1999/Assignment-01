using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentCRUD.Dtos;
using StudentCRUD.Repository;

namespace StudentCRUD.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpPost("students")]
        public async Task<IActionResult> Create([FromForm]StudentRequest student)
        {
            try
            {
                if(student is null)
                {
                    return BadRequest();
                }

                var isAdded = await _studentRepository.AddAsync(student);

                if (isAdded)
                {
                    return Ok();
                }

                return StatusCode(500,"Internal Server issue!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("students")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var listOfStudent = await _studentRepository.GetAllAsync();

                if (listOfStudent is null)
                {
                    return NotFound();
                }

                return Ok(listOfStudent);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("students/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);

                if (student is null)
                {
                    return NotFound();
                }

                return Ok(student);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("students/{id}")]
        public async Task<IActionResult> Update([FromForm]StudentRequest student , [FromRoute] long id)
        {
            try
            {
                if(student.Id != id)
                {
                    return BadRequest();
                }
                
                var isUpdated = await _studentRepository.UpdateAsync(student , id);

                if (isUpdated)
                {
                    return Ok(student);
                }

                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("students/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var isDeleted = await _studentRepository.DeleteAsync(id);

                if (isDeleted)
                {
                    return Ok();
                }

                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
