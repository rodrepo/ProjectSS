﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Comments = new List<CommentViewModel>();
        }

        public List<CommentViewModel> Comments { get; set; }
        public string TotalOnlineUsers { get; set; }
        public int TotalProjects { get; set; }
        public int TotalNumberOfItemsAssigned { get; set; }
        public int TotalPeddingRequst { get; set; }
    }
}