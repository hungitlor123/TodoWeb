using AutoMapper;
using TodoWeb.Application.DTOs;
using TodoWeb.Application.DTOs.Exam;
using TodoWeb.Application.DTOs.ExamResult;
using TodoWeb.Application.DTOs.Question;
using TodoWeb.Application.DTOs.UserModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Appllication.MapperProfiles;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        //Map course to CourseViewModel

        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.CourseId, x => x.MapFrom(src => src.Id))
            .ForMember(dest => dest.CourseName, x => x.MapFrom(src => src.Name))
            .ReverseMap();

        CreateMap<CourseCreateModel, Course>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CourseName));

        CreateMap<CourseUpdateModel, Course>()
            .ForMember(dest => dest.Name, x => x.PreCondition(src => !string.IsNullOrWhiteSpace(src.Name)));


        CreateMap<Student, StudentViewModel>()
            .ForMember(dest => dest.FullName, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.SchoolName, x => x.MapFrom(src => src.School.Name));

        CreateMap<Student, StudentCourseViewModel>()
            .ForMember(dest => dest.StudentId, x => x.MapFrom(src => src.Id))
            .ForMember(dest => dest.StudentName, x => x.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Courses, x => x.MapFrom(src => src.CourseStudents.Select(cs => cs.Course)));
        //Exam
        
        CreateMap<ExamCreateModel, Exam>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Exam, ExamViewModel>();
        CreateMap<ExamUpdateModel, Exam>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<StudentExamSubmission, ExamResult>();
        
        //Question
        CreateMap<QuestionCreateModel, QuestionBank>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<QuestionBank, QuestionViewModel>();
        CreateMap<QuestionUpdateModel, QuestionBank>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        //ExamResult
        CreateMap<ExamResultCreateModel, ExamResult>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<ExamResult, ExamResultViewModel>();
        CreateMap<ExamResultUpdateModel, ExamResult>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        //User
        CreateMap<UserCreateModel, User>();
    }
    
    
    
}