using System;
using System.Collections.Generic;

namespace WebApiBlazor.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public List<string> Images { get; set; }
    }
}