using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nager.Date;
using Nager.Date.Extensions;
using Nager.Date.Model;

namespace mtgroup.locacao.Interfaces
{
    public static class DateSystemUtils
    {
        public static DateTime NearestWorkDateBetween(DateTime min, DateTime max,
            bool includeBounds = true,
            string countryCode = "BR"
        )
        {
            if (!Enum.TryParse<CountryCode>(countryCode, true, out var dtCountryCode))
                throw new ArgumentException("Código de país inválido!", nameof(countryCode));

            DateTime tmpDate = includeBounds ? min : min.Date.AddDays(1);

            max = (includeBounds && max == DateTime.MaxValue) ? DateTime.MaxValue.AddDays(-1) : max;

            if (!includeBounds)
                max = max.AddDays(-1);

            IList <PublicHoliday> yearHolidays = Array.Empty<PublicHoliday>();
            PublicHoliday firstHoliday = null;

            var weekendProvider = DateSystem.GetWeekendProvider(dtCountryCode);
            
            do
            {
                if (weekendProvider.IsWeekend(tmpDate))
                {
                    tmpDate = tmpDate.AddDays(1);
                    continue;
                }

                if (firstHoliday?.Date.Year != tmpDate.Year)
                {
                    yearHolidays = DateSystem.GetPublicHoliday(tmpDate.Year, dtCountryCode).ToList();
                    firstHoliday = yearHolidays.FirstOrDefault();
                }

                if (!yearHolidays.Any(holiday => holiday.Date.Equals(tmpDate.Date)))
                    return tmpDate;
                
                tmpDate = tmpDate.AddDays(1);

            } while (tmpDate <= max);

            throw new InvalidOperationException("Não foi possível achar um dia de trabalho válido entre estas duas datas");
        }
        
        public static DateTime NearestWorkDate(DateTime reference, 
            bool includeSelf = true,
            string countryCode = "BR"
            )
        {
            return NearestWorkDateBetween(reference, DateTime.MaxValue, includeSelf, countryCode);
        }
    }
}
