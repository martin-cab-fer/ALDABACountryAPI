namespace ALDABACountryAPI.Models
{
    public class CountryData
    {
        public CountryData(DtoCountryData dto)
        {
            string name = dto.translations.Any(a => a.Key == "spa") ? dto.translations.FirstOrDefault(a => a.Key == "spa").Value.common : "";
            OfficialName = name + " (" + dto.name.official + ")";
            CapitalName = string.Join(", ", dto.capital);
            RegionName = dto.region;
            Population = dto.population;
            OfficialLanguages = dto.languages.Values.ToList();
            FlagURL = dto.flags.png;
        }

        public string OfficialName { get; set; }

        public string CapitalName { get; set; }

        public string RegionName { get; set; }

        public int Population { get; set; }

        public List<string> OfficialLanguages { get; set; } = new List<string>();

        public string FlagURL { get; set; }

        public string DataToText
        {
            get
            {
                string text = "Nombre oficial: " + OfficialName + Environment.NewLine;
                text += "Capital: " + CapitalName + Environment.NewLine;
                text += "Región: " + RegionName + Environment.NewLine;
                text += "Población: " + Population + Environment.NewLine;
                text += "Idiomas oficiales: " + string.Join(", ", OfficialLanguages) + Environment.NewLine;
                text += "Bandera: " + FlagURL;
                return text;
            }
        }
    }

    public class AdjacentCountryData
    {
        public AdjacentCountryData(DtoCountryData dto)
        {
            string name = dto.translations.Any(a => a.Key == "spa") ? dto.translations.FirstOrDefault(a => a.Key == "spa").Value.common : "";
            CountryName = name + " (" + dto.name.official + ")";
            CapitalName = string.Join(", ", dto.capital);
            Population = dto.population;
        }

        public string CountryName { get; set; }

        public string CapitalName { get; set; }

        public int Population { get; set; }

        public string DataToText {
            get
            {
                string text = "Nombre: " + CountryName + Environment.NewLine;
                text += "Capital: " + CapitalName + Environment.NewLine;
                text += "Población: " + Population;
                return text;
            }
        }
    }
}
