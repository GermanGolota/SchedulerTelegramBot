﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class SchedulesContext:DbContext
    {
        public SchedulesContext(DbContextOptions<SchedulesContext> options):base(options)
        {

        }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}