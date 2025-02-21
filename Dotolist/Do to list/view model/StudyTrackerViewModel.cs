using System.Collections.Generic;

namespace DoToList.Models
{
    public class StudyTrackerViewModel
    {
        public int StudyTime { get; set; }
        public int BreakTime { get; set; }
        public int OtherTime { get; set; }
        public List<StudySession> Sessions { get; set; }
    }
}
