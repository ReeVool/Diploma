using Diploma.Database.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

public class ManufacturersSearch
{
    [Obsolete]
    public IQueryable<Manufacturers> Search(string searchTerm, DbContext db)
    {
        var query = db.Set<Manufacturers>().AsQueryable();

        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        var predicate = PredicateBuilder.False<Manufacturers>();
        var properties = typeof(Manufacturers).GetProperties();

        foreach (var prop in properties)
        {
            var param = Expression.Parameter(typeof(Manufacturers), "x");
            var propExpr = Expression.Property(param, prop.Name);

            // Для строковых полей
            if (prop.PropertyType == typeof(string))
            {
                var body = Expression.Call(
                    typeof(DbFunctionsExtensions),
                    nameof(DbFunctionsExtensions.Like),
                    null,
                    Expression.Constant(EF.Functions),
                    propExpr,
                    Expression.Constant($"%{searchTerm}%")
                );
                predicate = predicate.Or(Expression.Lambda<Func<Manufacturers, bool>>(body, param));
            }
            // Для числовых полей
            else if (IsNumericType(prop.PropertyType))
            {
                if (TryConvertToType(searchTerm, prop.PropertyType, out object convertedValue))
                {
                    var equalsExpr = Expression.Equal(
                        propExpr,
                        Expression.Constant(convertedValue)
                    );
                    predicate = predicate.Or(Expression.Lambda<Func<Manufacturers, bool>>(equalsExpr, param));
                }
            }
        }

        return query.Where(predicate.Expand());
    }

    // Проверка числового типа
    private bool IsNumericType(Type type)
    {
        var numericTypes = new HashSet<Type>
        {
            typeof(int), typeof(double), typeof(decimal),
            typeof(long), typeof(short), typeof(float),
            typeof(uint), typeof(ulong), typeof(ushort),
            typeof(byte), typeof(sbyte)
        };

        var actualType = Nullable.GetUnderlyingType(type) ?? type;
        return numericTypes.Contains(actualType);
    }

    // Конвертация значений
    private bool TryConvertToType(string value, Type targetType, out object result)
    {
        result = null;
        try
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            result = Convert.ChangeType(value, underlyingType);
            return true;
        }
        catch
        {
            return false;
        }
    }
}