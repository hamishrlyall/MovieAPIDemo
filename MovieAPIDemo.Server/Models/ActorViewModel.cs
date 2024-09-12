﻿using System.ComponentModel.DataAnnotations;

namespace MovieAPIDemo.Server.Models
{
    public class ActorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
