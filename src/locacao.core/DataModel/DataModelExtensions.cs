namespace mtgroup.locacao.DataModel
{
    public static class DataModelExtensions
    {
        public static bool Atende(this IPerfilSalaReuniao self, RecursoSalaReuniao criterio)
        {
            if (criterio == RecursoSalaReuniao.Nenhum)
                return true;

            return (self.Recursos & criterio) != RecursoSalaReuniao.Nenhum;
        }
    }
}