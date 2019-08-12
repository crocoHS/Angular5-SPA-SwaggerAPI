﻿using System.ComponentModel.DataAnnotations;

namespace Todo.FunctionApp
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        [Required]
        public string OwnerId { get; set; }
    }
}
