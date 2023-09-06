using StudentCRUD.Dtos;

namespace StudentCRUD.Repository
{
    public interface IStudentRepository
    {
        Task<bool> AddAsync(StudentRequest student);

        Task<List<StudentResponse>> GetAllAsync();

        Task<StudentResponse> GetByIdAsync(long id);

        Task<bool> UpdateAsync(StudentRequest student , long id);

        Task<bool> DeleteAsync(long id);
    }
}
