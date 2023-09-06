using AutoMapper;
using StudentCRUD.Dtos;
using StudentCRUD.Model.Domain;

namespace StudentCRUD.Model.Helper
{
    public class MapperHandler :Profile
    {
        public MapperHandler()
        {
            CreateMap<Student, StudentRequest>().ReverseMap();
            CreateMap<Student, StudentResponse>().ReverseMap();
        }
    }
}
