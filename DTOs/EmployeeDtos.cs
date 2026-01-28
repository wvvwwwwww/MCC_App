namespace MccApi.DTOs
{
    public class EmployeeDtos
    {
        public class EmployeeReadDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? SecondName { get; set; }
            public string Number { get; set; } = string.Empty;
            public string PasportData { get; set; } = string.Empty;
            public int Standing { get; set; }
            public string? Photo { get; set; }
            public string? TitleName { get; set; }
            public int? TitleId { get; set; }
        }

        // DTO для создания
        public class EmployeeCreateDto
        {
            public string Name { get; set; } = string.Empty;
            public string? SecondName { get; set; }
            public string Number { get; set; } = string.Empty;
            public string PasportData { get; set; } = string.Empty;
            public int Standing { get; set; } = 0;
            public string? Photo { get; set; }
            public int? TitleId { get; set; }
        }

        // DTO для обновления
        public class EmployeeUpdateDto
        {
            public string Name { get; set; } = string.Empty;
            public string? SecondName { get; set; }
            public string Number { get; set; } = string.Empty;
            public string PasportData { get; set; } = string.Empty;
            public int Standing { get; set; }
            public string? Photo { get; set; }
            public int? TitleId { get; set; }
        }
    }

}
