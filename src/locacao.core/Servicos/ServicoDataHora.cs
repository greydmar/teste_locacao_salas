using System;
using System.Linq;
using mtgroup.locacao.Interfaces.Servicos;
using Nager.Date;

namespace mtgroup.locacao.Servicos
{
    public class ServicoDataHora : IServicoDataHora
    {
        public DateTime DataHoje => DataHoraHoje.Date;

        public DateTime DataHoraHoje => DateTime.Now;

        public bool EhDiaUtil(DateTime data)
        {
            if (DateSystem.IsWeekend(data, CountryCode.BR))
                return false;

            //TODO: Cache it!
            return !DateSystem.GetPublicHoliday(data, data, CountryCode.BR)
                .Any();
        }
    }
}