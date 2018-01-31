using System;

namespace Web.Models
{
    public class Links
    {
        public Self[] Self { get; set; }
        public Clone[] Clone { get; set; }
    }

    public class Self
    {
        public Uri Href { get; set; }
    }

    public class Clone
    {
        public Uri Href { get; set; }
        public string Name { get; set; }
    }
}