using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBlazor.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
    }
}