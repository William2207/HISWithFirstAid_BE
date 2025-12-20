using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace FirstAidAPI.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static PropertyBuilder<List<string>> HasJsonConversion(
            this PropertyBuilder<List<string>> propertyBuilder)
        {
            var comparer = new ValueComparer<List<string>>(
                // Fix null reference: handle null cases
                (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),

                // Fix aggregate: handle null and empty list
                c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),

                // Fix clone: handle null
                c => c == null ? new List<string>() : c.ToList()
            );

            propertyBuilder
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
                )
                .Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }
    }
}