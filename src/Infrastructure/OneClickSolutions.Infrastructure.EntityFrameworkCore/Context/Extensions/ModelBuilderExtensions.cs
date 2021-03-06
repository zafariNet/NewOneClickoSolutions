using System;
using System.Linq;
using System.Reflection;
using OneClickSolutions.Infrastructure.Common;
using OneClickSolutions.Infrastructure.Timing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace OneClickSolutions.Infrastructure.EntityFrameworkCore.Context.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void NormalizeDecimal(this ModelBuilder builder, int precision = 20, int scale = 6)
        {
            var propertyList = builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))
                .Where(property => !property.PropertyInfo.GetCustomAttributes<SkipNormalizationAttribute>().Any());

            foreach (var property in propertyList)
            {
                property.SetPrecision(precision);
                property.SetScale(scale);
            }
        }

        /// <summary>
        /// SpecifyKind of DateTime fields with DateTimeKind.Utc as a best-practice in web applications
        /// </summary>
        /// <param name="builder"></param>
        public static void NormalizeDateTime(this ModelBuilder builder)
        {
            var conversion = new ValueConverter<DateTime, DateTime>(
                v => v,
                v => SystemTime.Normalize(v));

            var propertyList = builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                .Where(property => property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                .Where(property => property.IsShadowProperty() ||
                                   !property.PropertyInfo.GetCustomAttributes<SkipNormalizationAttribute>().Any());

            foreach (var property in propertyList)
            {
                property.SetValueConverter(conversion);
            }
        }
    }
}