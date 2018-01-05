using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

namespace OliAzurePack
{
    // Windows Azure Table Storage does not support a StartsWith function
    // on string properties. However, it does support the CompareTo function.
    // This will allow us to simulate the StartsWith function by taking the
    // substring that you want to match on and figuring out the exclusive
    // upper bound by incrementing the last character.

    // from: https://gist.github.com/josheinstein/f405b293bc4bb1149f49

    public static class AzureTableExtensions
    {
        // Applies a predicate to the queryable that limits results to entities
        // that have a row key that begins with the specified substring.
        public static IQueryable<T> WhereRowKeyStartsWith<T>(this IQueryable<T> query, string startsWith) where T : ITableEntity
        {
            string upperBound = CreateUpperBoundString(startsWith);
            return query.Where(x =>
               x.RowKey.CompareTo(startsWith) >= 0 &&
               x.RowKey.CompareTo(upperBound) < 0
            );
        }

        // Applies a predicate to the queryable that limits results to entities
        // that have a partition key that begins with the specified substring.
        public static IQueryable<T> WherePartitionKeyStartsWith<T>(this IQueryable<T> query, string startsWith) where T : ITableEntity, new()
        {
            string upperBound = CreateUpperBoundString(startsWith);
            return query.Where(x =>
               x.PartitionKey.CompareTo(startsWith) >= 0 &&
               x.PartitionKey.CompareTo(upperBound) < 0
            );
        }

        // Given a string, returns a string that can be used as an upper bound in
        // a range query to achieve the same functionality as a StartsWith operator.
        // For example, passing ABCD as the lower bound will return ABCE as the upper
        // bound, so that the range would contain ABCD, ABCD0000, ABCDE, and everything
        // up until ABCE, which is the exclusive upper bound.
        private static string CreateUpperBoundString(string lowerBoundString)
        {

            if (String.IsNullOrEmpty(lowerBoundString))
            {
                throw new ArgumentException("lowerBoundString parameter cannot be a null or empty string.", "lowerBoundString");
            }

            // increment the last character of the inclusive lower bound
            // to get the exclusive upper bound.
            string upperBoundString = lowerBoundString.Substring(0, lowerBoundString.Length - 1) +
                (char)(lowerBoundString[lowerBoundString.Length - 1] + 1);

            return upperBoundString;

        }

    }
}
