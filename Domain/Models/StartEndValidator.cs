using System;

namespace Domain.Models
{
    public class StartEndValidator
    {
        public void StartEndValidation(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new Exception("Invalid time expression - end time cannot be earlier than start time");
            }
        }
    }
}
