﻿namespace PresentationLayer.DTO.Task
{
    public class TaskUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        
    }
}
