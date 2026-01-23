using MyCoffeeCupApp.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using static MyCoffeeCupApp.DTOs.EmployeeDtos;
using static MyCoffeeCupApp.DTOs.PointDtos;
using static MyCoffeeCupApp.DTOs.ScheduleDtos;

namespace MyCoffeeCupApp.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7231"),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        // ============== СОТРУДНИКИ ==============
        public async Task<List<EmployeeReadDto>> GetEmployeesAsync()
            => await _httpClient.GetFromJsonAsync<List<EmployeeReadDto>>("api/employees")
                ?? new List<EmployeeReadDto>();

        public async Task<EmployeeReadDto?> GetEmployeeByIdAsync(int id)
            => await _httpClient.GetFromJsonAsync<EmployeeReadDto>($"api/employees/{id}");

        public async Task<EmployeeReadDto?> CreateEmployeeAsync(EmployeeCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/employees", dto);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<EmployeeReadDto>()
                : null;
        }

        public async Task<bool> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/employees/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/employees/{id}");
            return response.IsSuccessStatusCode;
        }

        // ============== ТОЧКИ ==============
        public async Task<List<PointReadDto>> GetPointsAsync()
            => await _httpClient.GetFromJsonAsync<List<PointReadDto>>("api/points")
                ?? new List<PointReadDto>();

        public async Task<PointReadDto?> GetPointByIdAsync(int id)
            => await _httpClient.GetFromJsonAsync<PointReadDto>($"api/points/{id}");

        public async Task<PointReadDto?> CreatePointAsync(PointCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/points", dto);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<PointReadDto>()
                : null;
        }

        public async Task<bool> UpdatePointAsync(int id, PointUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/points/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePointAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/points/{id}");
            return response.IsSuccessStatusCode;
        }

        // ============== ДОЛЖНОСТИ ==============
        public async Task<List<TitleReadDto>> GetTitlesAsync()
            => await _httpClient.GetFromJsonAsync<List<TitleReadDto>>("api/titles")
                ?? new List<TitleReadDto>();

        // ============== РАСПИСАНИЕ ==============
        public async Task<List<ScheduleReadDto>> GetSchedulesAsync()
            => await _httpClient.GetFromJsonAsync<List<ScheduleReadDto>>("api/schedules")
                ?? new List<ScheduleReadDto>();

        public async Task<ScheduleReadDto?> CreateScheduleAsync(ScheduleCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/schedules", dto);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<ScheduleReadDto>()
                : null;
        }

        public async Task<bool> UpdateScheduleAsync(int id, ScheduleUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/schedules/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/schedules/{id}");
            return response.IsSuccessStatusCode;
        }

        // ============== ИЗМЕНЕНИЯ ==============
        public async Task<List<ChangeReadDto>> GetChangesAsync()
            => await _httpClient.GetFromJsonAsync<List<ChangeReadDto>>("api/changes")
                ?? new List<ChangeReadDto>();

        public async Task<ChangeReadDto?> CreateChangeAsync(ChangeCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/changes", dto);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<ChangeReadDto>()
                : null;
        }

        public async Task<bool> UpdateChangeAsync(int id, ChangeUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/changes/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteChangeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/changes/{id}");
            return response.IsSuccessStatusCode;
        }

        // ============== СТАТУСЫ ИЗМЕНЕНИЙ ==============
        public async Task<List<ChangeStatusReadDto>> GetChangeStatusesAsync()
            => await _httpClient.GetFromJsonAsync<List<ChangeStatusReadDto>>("api/changestatuses")
                ?? new List<ChangeStatusReadDto>();

        // ============== ИНДИВИДУАЛЬНОЕ РАСПИСАНИЕ ==============
        public async Task<List<EmployeeScheduleReadDto>> GetEmployeeSchedulesAsync()
            => await _httpClient.GetFromJsonAsync<List<EmployeeScheduleReadDto>>("api/employeeschedules")
                ?? new List<EmployeeScheduleReadDto>();

        public async Task<EmployeeScheduleReadDto?> CreateEmployeeScheduleAsync(EmployeeScheduleCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/employeeschedules", dto);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<EmployeeScheduleReadDto>()
                : null;
        }

        // ============== ДНИ НЕДЕЛИ ==============
        public async Task<List<WeekDayReadDto>> GetDaysOfWeekAsync()
            => await _httpClient.GetFromJsonAsync<List<WeekDayReadDto>>("api/daysofweek")
                ?? new List<WeekDayReadDto>();

        // ============== СОВЕЩАНИЯ ==============
        public async Task<List<MeetingReadDto>> GetMeetingsAsync()
            => await _httpClient.GetFromJsonAsync<List<MeetingReadDto>>("api/meetings")
                ?? new List<MeetingReadDto>();

        public async Task<MeetingReadDto?> CreateMeetingAsync(MeetingCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/meetings", dto);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<MeetingReadDto>()
                : null;
        }

        // ============== ТЕМЫ СОВЕЩАНИЙ ==============
        public async Task<List<MeetingTopicReadDto>> GetMeetingTopicsAsync()
            => await _httpClient.GetFromJsonAsync<List<MeetingTopicReadDto>>("api/meetingtopics")
                ?? new List<MeetingTopicReadDto>();

        // ============== СТАТУСЫ СОВЕЩАНИЙ ==============
        public async Task<List<MeetingStatusReadDto>> GetMeetingStatusesAsync()
            => await _httpClient.GetFromJsonAsync<List<MeetingStatusReadDto>>("api/meetingstatuses")
                ?? new List<MeetingStatusReadDto>();



        // ============== ИСТОРИЯ ИЗМЕНЕНИЙ ==============
        public async Task<List<ChangesHistoryReadDto>> GetChangesHistoryAsync()
            => await _httpClient.GetFromJsonAsync<List<ChangesHistoryReadDto>>("api/changeshistory")
                ?? new List<ChangesHistoryReadDto>();

        // ============== АВТОРИЗАЦИЯ ==============
        public async Task<List<AutorizationReadDto>> GetAutorizationsAsync()
            => await _httpClient.GetFromJsonAsync<List<AutorizationReadDto>>("api/autorizations")
                ?? new List<AutorizationReadDto>();

        // ============== РОЛИ ==============
        public async Task<List<RolesReadDto>> GetRolesAsync()
            => await _httpClient.GetFromJsonAsync<List<RolesReadDto>>("api/roles")
                ?? new List<RolesReadDto>();

        // ============== АВТОРИЗАЦИЯ ПОЛЬЗОВАТЕЛЯ ==============
        public async Task<LoginResponseDto?> LoginAsync(string username, string password)
        {
            var loginDto = new { username, password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
                }
                return result;
            }
            return null;
        }

        public void Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    // Класс для ответа от авторизации
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}