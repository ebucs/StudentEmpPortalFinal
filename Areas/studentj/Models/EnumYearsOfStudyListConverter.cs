using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq;
using static StudentEmploymentPortal.Models.JobPost;

public class EnumYearsOfStudyListConverter : ValueConverter<List<EnumYearsOfStudy>, string>
{
    public EnumYearsOfStudyListConverter() : base(
        v => string.Join(",", v),
        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => (EnumYearsOfStudy)Enum.Parse(typeof(EnumYearsOfStudy), s))
            .ToList())
    {
    }
}
