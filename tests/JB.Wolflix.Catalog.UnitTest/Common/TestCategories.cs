using JB.Wolflix.Catalog.Domain.Entity;

namespace JB.Wolflix.Catalog.UnitTest.Common
{
    public static class TestCategories
    {
        // Chaves de categorias (primeiro parâmetro do Trait)
        public const string Layer = "Camada";
        public const string Domain = "Domínio";
        public const string Scenery = "Cenário";

        // Valores de traits (segundo parâmetro do Trait)
        public const string CategoryName = "Categoria";
        public const string Aggregate = "Agregado";
        public const string CategoryAggregate = "Categoria - Agregado";

        // Dicionário com textos de exibição (pt-BR)
        public static readonly Dictionary<string, string> All = new()
        {
            { Layer, "Teste de Camadas" },
            { Domain, "Teste de Domínio" },
            { Scenery, "Cenário de Teste" },
            { CategoryName, "Categoria" },
            { Aggregate, "Agregado de Domínio" }
        };

    }
}
