using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SAA.Tests;

public class DomainCoverageTests
{
    [Fact]
    public void AllDomainEntities_Properties_CanBeSetAndGet()
    {
        // Get all types in SAA.Domain
        var domainAssembly = typeof(SAA.Domain.Entities.Usuario).Assembly;
        var types = domainAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null && t.Namespace.Contains("Entities"));

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            instance.Should().NotBeNull();

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var prop in properties)
            {
                object? testValue = GetTestValueForType(prop.PropertyType);
                prop.SetValue(instance, testValue);
                
                var retrievedValue = prop.GetValue(instance);
                retrievedValue.Should().Be(testValue, $"Property {prop.Name} on {type.Name} failed");
            }
        }
    }

    private object? GetTestValueForType(Type type)
    {
        if (type == typeof(string)) return "test";
        if (type == typeof(int) || type == typeof(int?)) return 1;
        if (type == typeof(decimal) || type == typeof(decimal?)) return 1.5m;
        if (type == typeof(DateTime) || type == typeof(DateTime?)) return new DateTime(2025, 1, 1);
        if (type == typeof(TimeSpan) || type == typeof(TimeSpan?)) return new TimeSpan(1, 0, 0);
        if (type == typeof(bool) || type == typeof(bool?)) return true;
        
        if (type.IsValueType) return Activator.CreateInstance(type);
        
        return null; // For complex objects, skip setting or just set null
    }
}
