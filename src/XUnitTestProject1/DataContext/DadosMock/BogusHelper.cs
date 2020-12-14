using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bogus;
using mtgroup.locacao.DataModel;

namespace locacao.tests.DadosMock
{
    internal static class BogusHelper
    {
        //https://stackoverflow.com/a/6117125
        private static IEnumerable<T> AllCombinations<T>() where T : struct
        {
            // Constuct a function for OR-ing together two enums
            Type type = typeof(T);
            var param1 = Expression.Parameter(type);
            var param2 = Expression.Parameter(type);
            var orFunction = Expression.Lambda<Func<T, T, T>>(
                Expression.Convert(
                    Expression.Or(
                        Expression.Convert(param1, type.GetEnumUnderlyingType()),
                        Expression.Convert(param2, type.GetEnumUnderlyingType())),
                    type), param1, param2).Compile();

            var initialValues = (T[]) Enum.GetValues(type);
            var discoveredCombinations = new HashSet<T>(initialValues);
            var queue = new Queue<T>(initialValues);

            // Try OR-ing every inital value to each value in the queue
            while (queue.Count > 0)
            {
                T a = queue.Dequeue();
                foreach (T b in initialValues)
                {
                    T combo = orFunction(a, b);
                    if (discoveredCombinations.Add(combo))
                        queue.Enqueue(combo);
                }
            }

            return discoveredCombinations;
        }
        
        static IEnumerable<RecursoSalaReuniao> CombinacoesDeRecursos()
        {
            var seed = new[]
            {
                RecursoSalaReuniao.Nenhum,
                RecursoSalaReuniao.Computador,
                RecursoSalaReuniao.Televisor,
                RecursoSalaReuniao.AcessoInternet,
                RecursoSalaReuniao.VideoConferencia
            };

            return AllCombinations<RecursoSalaReuniao>();
        }

        public static IEnumerable<RequisicaoSalaReuniao> Gerador(
            DateTime dataReferencia, int numeroElementos)
        {
            var recursos = CombinacoesDeRecursos().ToArray();

            var gerador = new Faker<RequisicaoSalaReuniao>()
                .RuleFor(o => o.QuantidadePessoas, faker => faker.Random.UShort(1, 30))
                .RuleFor(o => o.Recursos, (faker) => faker.Random.ArrayElement(recursos))
                .RuleFor(o => o.Periodo, faker =>
                {
                    var dataZero = dataReferencia.Date;
                    var dataInicio = faker.Date.Between(dataZero, dataZero.AddDays(60));
                    var horasDuracao = faker.Random.UShort(0, 24);

                    return new PeriodoLocacao(dataInicio, TimeSpan.FromHours(horasDuracao));
                });

            return gerador.Generate(numeroElementos);
        }
    }
}