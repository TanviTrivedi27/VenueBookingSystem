using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MetronicsAdminPanel.Models
{
    public partial class Sliders
    {
        [Key]
        public int SliderId { get; set; }
        public string? ImgPath { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? BtnTitle { get; set; }
        public string? BtnRedirectUrl { get; set; }
    }
}
