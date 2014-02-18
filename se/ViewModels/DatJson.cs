using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using se.Models;

namespace se.ViewModels
{
    public class DatJson
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public List<PointsJson> Points { get; set; }
    }
}