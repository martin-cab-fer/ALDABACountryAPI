namespace ALDABACountryAPI.Models
{
    public class DtoCountryData
    {
        public DtoCountryNames name { get; set; }

        public List<string> capital { get; set; }

        public int population { get; set; }

        public string region { get; set; }

        public Dictionary<string, string> languages { get; set; }

        public List<string> borders { get; set; }

        public DtoCountryFlags flags { get; set; }

        public Dictionary<string, DtoCountryNames> translations { get; set; }
    }

    public class DtoCountryNames
    {
        public string common { get; set; }

        public string official { get; set; }
    }

    public class DtoCountryFlags
    {
        public string svg { get; set; }

        public string png { get; set; }
    }
}
