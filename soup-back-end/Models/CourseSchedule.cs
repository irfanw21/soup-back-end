namespace soup_back_end.Models
{
    public class CourseSchedule
    {
        public string scheduleId { get; set; } = string.Empty;
        public DateTime scheduleDate { get; set; }
        public string courseId { get; set; } = string.Empty;
    }
}
