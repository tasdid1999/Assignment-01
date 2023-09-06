using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using StudentCRUD.DataAccess;
using StudentCRUD.Dtos;
using StudentCRUD.Model.Domain;
using StudentCRUD.Services;

namespace StudentCRUD.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly SqlConnectionFactory _sqlConnectionFactory; 

        public StudentRepository(ApplicationDbContext dbcontext , IMapper mapper,SqlConnectionFactory sqlConnectionFactory)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<bool> AddAsync(StudentRequest student)
        {
            var DbStudent = _mapper.Map<Student>(student);

            await _dbcontext.students.AddAsync(DbStudent);

            var dbActionResult = await _dbcontext.SaveChangesAsync();

            return dbActionResult > 0 ? true : false;
        }

        public  async Task<bool> DeleteAsync(long id)
        {
            var dbStudent = await _dbcontext.students
                                            .Where(x => x.Id == id)
                                            .FirstOrDefaultAsync();
            if(dbStudent is not null)
            {
                 _dbcontext.students.Remove(dbStudent);

                var dbActionResult = await _dbcontext.SaveChangesAsync();

                return dbActionResult > 0 ? true : false;

            }

            return false;
        }

        public async Task<List<StudentResponse>> GetAllAsync()
        {
            var listOfStudent = await _dbcontext.students
                                                .AsNoTracking()
                                                .ToListAsync();

            var studentResponseList = _mapper.Map<List<StudentResponse>>(listOfStudent);

            return studentResponseList;
        }

        public async Task<StudentResponse> GetByIdAsync(long id)
        {
            using var connection = _sqlConnectionFactory.Create();

            const string sql = "SELECT Id,Name,Department FROM students WHERE Id = @Studentid";

            var student = await connection.QueryFirstOrDefaultAsync<Student>(sql, new { Studentid = id });

            return _mapper.Map<StudentResponse>(student);

        }

                
        public async Task<bool> UpdateAsync(StudentRequest student ,long id)
        {
            var updatedStudent = _mapper.Map<Student>(student);

            _dbcontext.students.Update(updatedStudent);

            var dbActionResult = await _dbcontext.SaveChangesAsync();

            return dbActionResult > 0 ? true : false;

        }
    }
}
