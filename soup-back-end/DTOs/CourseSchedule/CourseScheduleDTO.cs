namespace soup_back_end.DTOs.CourseSchedule
{
    public class CourseScheduleDTO
    {
        public string scheduleId { get; set; } = string.Empty;
        public DateTime scheduleDate { get; set; }
        public string courseId { get; set; } = string.Empty;
    }
}
