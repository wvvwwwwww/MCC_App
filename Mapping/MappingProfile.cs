using AutoMapper;
using MccApi.DTOs;
using MccApi.Models;
using static MccApi.DTOs.EmployeeDtos;
using static MccApi.DTOs.PointDtos;
using static MccApi.DTOs.ScheduleDtos;
namespace MccApi.Mapping
{
    public class MappingProfile : Profile
    {
            public MappingProfile()
            {
                // Employee
                CreateMap<Employee, EmployeeReadDto>()
                    .ForMember(dest => dest.TitleName,
                        opt => opt.MapFrom(src => src.Title != null ? src.Title.TitleName : null));
                CreateMap<EmployeeCreateDto, Employee>();
                CreateMap<EmployeeUpdateDto, Employee>();

                // Title
                CreateMap<Title, TitleReadDto>();
                CreateMap<TitleCreateDto, Title>();
                CreateMap<TitleUpdateDto, Title>();

                // Point
                CreateMap<Point, PointReadDto>()
                    .ForMember(dest => dest.GeneralEmployeeName,
                        opt => opt.MapFrom(src => src.GeneralEmployee.Name));
                CreateMap<PointCreateDto, Point>();
                CreateMap<PointUpdateDto, Point>();

                // Schedule
                CreateMap<Schedule, ScheduleReadDto>()
                    .ForMember(dest => dest.DayOfWeekName,
                        opt => opt.MapFrom(src => src.DayOfWeek.DayOfWeekName))
                    .ForMember(dest => dest.EmployeeName,
                        opt => opt.MapFrom(src => src.Employee.Name))
                    .ForMember(dest => dest.PointAddress,
                        opt => opt.MapFrom(src => src.Point.Address));
                CreateMap<ScheduleCreateDto, Schedule>();
                CreateMap<ScheduleUpdateDto, Schedule>();

                // EmployeeSchedule
                CreateMap<EmployeeSchedule, EmployeeScheduleReadDto>()
                    .ForMember(dest => dest.EmployeeName,
                        opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Name : null));
                CreateMap<EmployeeScheduleCreateDto, EmployeeSchedule>();
                CreateMap<EmployeeScheduleUpdateDto, EmployeeSchedule>();

                // Change
                CreateMap<Change, ChangeReadDto>()
                    .ForMember(dest => dest.EmployeeName,
                        opt => opt.MapFrom(src => src.Employee.Name))
                    .ForMember(dest => dest.StatusName,
                        opt => opt.MapFrom(src => src.ChangeStatus.StatusName));
                CreateMap<ChangeCreateDto, Change>();
                CreateMap<ChangeUpdateDto, Change>();

                // ChangeStatus
                CreateMap<ChangeStatus, ChangeStatusReadDto>();
                CreateMap<ChangeStatusCreateDto, ChangeStatus>();
                CreateMap<ChangeStatusUpdateDto, ChangeStatus>();

                // Meeting
                CreateMap<Meeting, MeetingReadDto>()
                    .ForMember(dest => dest.PointAddress,
                        opt => opt.MapFrom(src => src.Point.Address))
                    .ForMember(dest => dest.StatusName,
                        opt => opt.MapFrom(src => src.MeetingStatus != null ? src.MeetingStatus.StatusName : null))
                    .ForMember(dest => dest.TopicName,
                        opt => opt.MapFrom(src => src.MeetingTopic.TopicName))
                    .ForMember(dest => dest.Attendees,
                        opt => opt.MapFrom(src => src.MeetingAttends.Select(ma => new MeetingAttendeeDto
                        {
                            EmployeeId = ma.EmployeeId,
                            EmployeeName = ma.Employee.Name
                        })));
                CreateMap<MeetingCreateDto, Meeting>();
                CreateMap<MeetingUpdateDto, Meeting>();

                // MeetingTopic
                CreateMap<MeetingTopic, MeetingTopicReadDto>();
                CreateMap<MeetingTopicCreateDto, MeetingTopic>();
                CreateMap<MeetingTopicUpdateDto, MeetingTopic>();

                // MeetingStatus
                CreateMap<MeetingStatus, MeetingStatusReadDto>();
                CreateMap<MeetingStatusCreateDto, MeetingStatus>();
                CreateMap<MeetingStatusUpdateDto, MeetingStatus>();

                // ChangesHistory
                CreateMap<ChangesHistory, ChangesHistoryReadDto>()
                    .ForMember(dest => dest.EmployeeName,
                        opt => opt.MapFrom(src => src.Employee.Name))
                    .ForMember(dest => dest.PointAddress,
                        opt => opt.MapFrom(src => src.Point.Address));
                CreateMap<ChangesHistoryCreateDto, ChangesHistory>();
                CreateMap<ChangesHistoryUpdateDto, ChangesHistory>();

                // WeekDay (бывший DayOfWeek)
                CreateMap<WeekDay, WeekDayReadDto>();
                CreateMap<WeekDayCreateDto, WeekDay>();
                CreateMap<WeekDayUpdateDto, WeekDay>();

                // Autorization
                CreateMap<Autorization, AutorizationReadDto>()
                    .ForMember(dest => dest.EmployeeName,
                        opt => opt.MapFrom(src => src.Employee.Name))
                    .ForMember(dest => dest.RoleName,
                        opt => opt.MapFrom(src => src.Role.RoleName));
                CreateMap<AutorizationCreateDto, Autorization>();
                CreateMap<AutorizationUpdateDto, Autorization>();

                // Roles
                CreateMap<Roles, RolesReadDto>();
                CreateMap<RolesCreateDto, Roles>();
                CreateMap<RolesUpdateDto, Roles>();

                // MeetingAttend
                CreateMap<MeetingAttend, MeetingAttendeeDto>()
                    .ForMember(dest => dest.EmployeeId,
                        opt => opt.MapFrom(src => src.EmployeeId))
                    .ForMember(dest => dest.EmployeeName,
                        opt => opt.MapFrom(src => src.Employee.Name));
            }
        }
    }
